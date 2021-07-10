using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Network.Services;
using GameServer.MapGeneration;
using GameServer.Models;
using GameServer.Models.Units;
using GameServer.NetworkShared;
using GameServer.NetworkShared.Packets.World.ServerClient;
using GameServer.Utilities;
using Newtonsoft.Json;

namespace GameServer.Managers
{
    public class GameManager
    {
        private static GameManager _instance;
        private IMapGenerator _mapGenerator;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }

                return _instance;
            }
        }

        private IDictionary<int, Game> _games;

        private int _lastGameId;

        public int GamesCount { get { return _games.Count; } }

        public void Initialize()
        {
            _games = new Dictionary<int, Game>();
            _mapGenerator = new MapGenerator();
            _lastGameId = 0;

            GenerateDummyGame();
        }

        private void GenerateDummyGame()
        {
            var gameString = File.ReadAllText("DummyData/DummyGame.json");
            var game = JsonConvert.DeserializeObject<Game>(gameString);
            // TODO: Link users
            // TODO: Link avatars
            // TODO NOTE: Users do not exist when dummy game is loaded
            // thats why i need to populate the user reference after the user login to the server.
            // do the change in AuthRequestHandler.cs
            RegisterGame(game);

            var nzhul = RequestManagerHttp.UsersService.GetUser(3);
            var freda = RequestManagerHttp.UsersService.GetUser(1);

            RelinkGameUserAndAvatarTMP(nzhul);
            RelinkGameUserAndAvatarTMP(freda);

            nzhul.Avatar.IsDisconnected = true;
            freda.Avatar.IsDisconnected = true;

            Console.WriteLine("Adding 1 dummy game!");
        }

        public bool GameIsRegistered(int gameId)
        {
            return _games.ContainsKey(gameId);
        }

        public void RegisterGame(Game game)
        {
            _games.Add(game.Id, game);
            _lastGameId = game.Id;
        }

        public Game GetGameByConnectionId(int connectionId)
        {
            var gameId = this.GetGameIdByConnectionId(connectionId);
            if (!gameId.HasValue)
            {
                Console.WriteLine($"No active game was found for connectionId: {connectionId}");
                return null;
            }

            return _games[gameId.Value];
        }

        public Game GetGameByUserId(int userId)
        {
            var gameId = this.GetGameIdByUserId(userId);
            if (!gameId.HasValue)
            {
                Console.WriteLine($"No active game was found for userId: {userId}");
                return null;
            }

            return _games[gameId.Value];
        }

        public Unit GetUnit(int gameId, int unitId)
        {
            var game = _games[gameId];

            foreach (var army in game.Armies)
            {
                var unit = army.Units.FirstOrDefault(x => x.Id == unitId);
                if (unit != null)
                {
                    return unit;
                }
            }

            return null;
        }

        public Army GetArmy(int gameId, int armyId)
        {
            var game = _games[gameId];
            return game.Armies.FirstOrDefault(x => x.Id == armyId);
        }

        public int? GetGameIdByConnectionId(int connectionId)
        {
            return NetworkServer.Instance.Connections[connectionId].GameId;
        }

        public int GetConnectionIdByArmyId(int gameId, int armyId)
        {
            var army = _games[gameId].Armies.FirstOrDefault(x => x.Id == armyId);
            var connection = NetworkServer.Instance.Connections.FirstOrDefault(x => x.Value.UserId == army.UserId);
            return connection.Value != null ? connection.Value.ConnectionId : -1;
        }

        //public Unit GetRandomAvailibleUnit(Army army)
        //{
        //    var availibleUnits = army.Units.Where(x => !x.ActionConsumed).ToList();
        //    return availibleUnits[RandomGenerator.RandomNumber(0, army.Units.Count - 1)]; // TODO: Not tested
        //}

        public void DisconnectFromGame(int userId)
        {
            var gameId = GetGameIdByUserId(userId);
            if (!gameId.HasValue)
            {
                return;
            }

            var avatar = _games[gameId.Value].Avatars.FirstOrDefault(x => x.UserId == userId);
            avatar.IsDisconnected = true;
        }

        public void LeaveGame(ServerConnection connection)
        {
            int gameId;
            if (!connection.GameId.HasValue)
            {
                Console.WriteLine("[ERROR] Leaving game. ServerConnection gameId is null");
            }

            gameId = connection.GameId.Value;

            var avatar = _games[gameId].Avatars.FirstOrDefault(x => x.UserId == connection.UserId);
            avatar.HasLeftTheGame = true;

            // If there are only two player left in the game, the last player standing is the winner.
            var playersCount = _games[gameId].Avatars.Count(x => !x.HasLeftTheGame);
            if (playersCount == 1)
            {
                var winnerAvatar = _games[gameId].Avatars.FirstOrDefault(x => !x.HasLeftTheGame);
                var winnerConnectionId = GetConnectionIdByUserId(winnerAvatar.UserId);

                if (winnerConnectionId >= 0)
                {
                    // Notify winner
                    var msg = new Net_OnEndGameEvent() { WinnerId = winnerAvatar.UserId }; // doesn't exist yet.
                    NetworkServer.Instance.Send(winnerConnectionId, msg);
                }

                HttpUtilities.FF(() =>
                {
                    RequestManagerHttp.GameService.EndGame(gameId, winnerAvatar.UserId);
                }, $"Error ending game with id: {gameId}");

                // 3. Remove the game from the pool.
                //_games.Remove(gameId);

                return;
            }


            // all players have left the game. we can close it now.
            // game statistics is already recorded on previous stage.
            if (playersCount == 0)
            {
                _games.Remove(gameId);
            }

            // TODO: 1. TCP call to let other players know that this player has left the game
        }

        public Game CreateGame(GameParams @params)
        {
            Map generatedMap = _mapGenerator.TryGenerateMap(@params);
            _lastGameId++;

            var newGame = new Game()
            {
                Id = _lastGameId,
                Avatars = InitAvatars(@params),
                MatrixString = generatedMap.MatrixString,
                Dwellings = generatedMap.Dwellings,
                Treasures = generatedMap.Treasures,
                Armies = generatedMap.Armies,
            };

            AssignAvatarsToEntities(newGame);

            AssignGameToUsers(newGame.Id, @params.Players.Select(x => x.UserId));

            return newGame;
        }

        private void AssignGameToUsers(int gameId, IEnumerable<int> userIds)
        {
            foreach (var userId in userIds)
            {
                var con = NetworkServer.Instance.Connections.FirstOrDefault(x => x.Value.UserId == userId).Value;
                con.GameId = gameId;
            }
        }

        private void AssignAvatarsToEntities(Game newGame)
        {
            var teams = (Team[])Enum.GetValues(typeof(Team));

            for (int i = 1; i < teams.Length; i++)
            {
                var team = teams[i];

                var avatarsFromTeam = newGame.Avatars.Where(x => x.Team == team);
                var armies = newGame.Armies.Where(x => x.Team == team && !x.IsNPC && x.UserId == null);
                var dwellings = newGame.Dwellings.Where(x => x.Team == team && x.UserId == null);

                foreach (var avatar in avatarsFromTeam)
                {
                    var availibleArmy = armies.FirstOrDefault(x => x.Team == avatar.Team);
                    var availibleCastle = dwellings.FirstOrDefault(x => x.Team == avatar.Team
                        && x.Type == DwellingType.Castle && x.Link == availibleArmy.Link);

                    availibleArmy.UserId = avatar.UserId;
                    availibleArmy.User = avatar.User;
                    availibleArmy.Avatar = avatar;

                    foreach (var unit in availibleArmy.Units)
                    {
                        unit.UserId = avatar.UserId;
                    }

                    availibleCastle.UserId = avatar.UserId;
                    availibleCastle.User = avatar.User;
                    //TODO: we are currently handling only heroes and castles. Handle other dwellings if needed.
                }
            }
        }

        private IList<Avatar> InitAvatars(GameParams @params)
        {
            var avatars = new List<Avatar>();
            foreach (var player in @params.Players)
            {
                var user = NetworkServer.Instance.GetUser(player.UserId);
                var newAvatar = new Avatar
                {
                    UserId = player.UserId,
                    User = user,
                    Team = player.Team
                };

                user.Avatar = newAvatar;

                avatars.Add(newAvatar);
            }

            return avatars;
        }

        private int GetConnectionIdByUserId(int userId)
        {
            var connection = NetworkServer.Instance.Connections.FirstOrDefault(x => x.Value.UserId == userId);
            if (connection.Equals(default(KeyValuePair<int, ServerConnection>))) // null check for kvpair
            {
                return -1;
            }

            return NetworkServer.Instance.Connections.FirstOrDefault(x => x.Value.UserId == userId).Value.ConnectionId;
        }

        public int? GetGameIdByUserId(int userId)
        {
            var gameKv = _games.FirstOrDefault(x => x.Value.Avatars.Any(y => y.UserId == userId));
            if (gameKv.Equals(default(KeyValuePair<int, Game>)))
            {
                return null;
            }

            return gameKv.Value.Id;
        }

        public void RelinkGameUserAndAvatarTMP(Models.Users.User user)
        {
            var game = GetGameByUserId(user.Id);
            if (game == null)
            {
                return;
            }

            foreach (var avatar in game.Avatars)
            {
                if (avatar.UserId == user.Id)
                {
                    avatar.User = user;
                    user.Avatar = avatar;
                }
            }

            foreach (var army in game.Armies)
            {
                if (army.UserId == user.Id)
                {
                    army.User = user;
                    army.Avatar = game.Avatars.FirstOrDefault(x => x.UserId == user.Id);
                }
            }
        }

        public void DoNewDayTimeCheck(TimeSpan tickInterval)
        {
            foreach (var kv in _games)
            {
                var game = kv.Value;

                if (game.TimerStopped)
                {
                    game.PauseTime.Add(new TimeSpan(0, 0, tickInterval.Seconds));
                    continue;
                }

                if (game.CurrentDayStartTime + Constants.DAY_DURATION + game.PauseTime < DateTime.UtcNow)
                {
                    TriggerNextDay(game);
                }
            }
        }

        public void DoNewDayRestCheck(int userId)
        {
            var game = GetGameByUserId(userId);
            var allPlayersResting = game.Avatars.All(x => x.IsResting);
            if (allPlayersResting)
            {
                TriggerNextDay(game);
            }

        }

        private static void TriggerNextDay(Game game)
        {
            game.TotalDays++;
            game.CurrentDayStartTime = DateTime.UtcNow;

            var msg = new Net_OnNewDay()
            {
                Day = game.Day,
                Week = game.Week,
                Month = game.Month,
                TotalDays = game.TotalDays
            };

            var recipients = game.Avatars.Where(x => !x.IsDisconnected).Select(x => x.User.Connection);
            foreach (var recipient in recipients)
            {
                NetworkServer.Instance.Send(recipient.ConnectionId, msg);
            }

            // restore players movement points and IsResting
            foreach (var avatar in game.Avatars)
            {
                avatar.IsResting = false;
            }

            foreach (var army in game.Armies)
            {
                army.MovementPoints = army.MaxMovementPoints;
            }
        }
    }
}
