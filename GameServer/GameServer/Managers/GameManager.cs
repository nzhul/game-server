using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Network.Services;
using GameServer.MapGeneration;
using GameServer.Models;
using GameServer.Models.Units;
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
            RegisterGame(game);
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
            if (gameId == 0)
            {
                Console.WriteLine($"No active game was found for connectionId: {connectionId}");
            }

            return _games[gameId];
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

        public int GetGameIdByConnectionId(int connectionId)
        {
            return NetworkServer.Instance.Connections[connectionId].GameId;
        }

        public int GetConnectionIdByArmyId(int gameId, int armyId)
        {
            var army = _games[gameId].Armies.FirstOrDefault(x => x.Id == armyId);
            var connection = NetworkServer.Instance.Connections.FirstOrDefault(x => x.Value.UserId == army.UserId);
            return connection.Value != null ? connection.Value.ConnectionId : 0;
        }

        public Unit GetRandomAvailibleUnit(Army army)
        {
            var availibleUnits = army.Units.Where(x => !x.ActionConsumed).ToList();
            return availibleUnits[RandomGenerator.RandomNumber(0, army.Units.Count - 1)]; // TODO: Not tested
        }

        public void DisconnectFromGame(int userId)
        {
            var gameId = GetGameIdByUserId(userId);
            var avatar = _games[gameId].Avatars.FirstOrDefault(x => x.UserId == userId);
            avatar.IsDisconnected = true;
        }

        public void LeaveGame(ServerConnection connection)
        {
            var gameId = connection.GameId;
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
                _games.Remove(gameId);

                return;
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
                    availibleCastle.UserId = avatar.UserId;
                    //TODO: we are currently handling only heroes and castles. Handle other dwellings if needed.
                }
            }
        }

        private IList<Avatar> InitAvatars(GameParams @params)
        {
            var avatars = new List<Avatar>();
            foreach (var player in @params.Players)
            {
                var newAvatar = new Avatar
                {
                    UserId = player.UserId,
                    Team = player.Team
                };

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

        private int GetGameIdByUserId(int userId)
        {
            return _games.FirstOrDefault(x => x.Value.Avatars.Any(y => y.UserId == userId)).Value.Id;
        }
    }
}
