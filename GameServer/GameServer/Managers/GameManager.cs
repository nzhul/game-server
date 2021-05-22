using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Network.Services;
using GameServer.Models;
using GameServer.Models.Units;
using GameServer.NetworkShared.Packets.World.ServerClient;
using GameServer.Utilities;

namespace GameServer.Managers
{
    public class GameManager
    {
        private static GameManager _instance;

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

        private IDictionary<int, Game> Games;

        public void Initialize()
        {
            this.Games = new Dictionary<int, Game>();
        }

        public bool GameIsRegistered(int gameId)
        {
            return this.Games.ContainsKey(gameId);
        }

        public void RegisterGame(Game game)
        {
            this.Games.Add(game.Id, game);
        }

        public Game GetGameByConnectionId(int connectionId)
        {
            var gameId = this.GetGameIdByConnectionId(connectionId);
            if (gameId == 0)
            {
                Console.WriteLine($"No active game was found for connectionId: {connectionId}");
            }

            return this.Games[gameId];
        }

        public Unit GetUnit(int gameId, int unitId)
        {
            var game = this.Games[gameId];

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
            var game = this.Games[gameId];
            return game.Armies.FirstOrDefault(x => x.Id == armyId);
        }

        public int GetGameIdByConnectionId(int connectionId)
        {
            return NetworkServer.Instance.Connections[connectionId].GameId;
        }

        public int GetConnectionIdByArmyId(int gameId, int armyId)
        {
            var army = this.Games[gameId].Armies.FirstOrDefault(x => x.Id == armyId);
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
            var avatar = this.Games[gameId].Avatars.FirstOrDefault(x => x.UserId == userId);
            avatar.IsDisconnected = true;
        }

        public void LeaveGame(ServerConnection connection)
        {
            var gameId = connection.GameId;
            var avatar = this.Games[gameId].Avatars.FirstOrDefault(x => x.UserId == connection.UserId);
            avatar.HasLeftTheGame = true;

            // If there are only two player left in the game, the last player standing is the winner.
            var playersCount = this.Games[gameId].Avatars.Count(x => !x.HasLeftTheGame);
            if (playersCount == 1)
            {
                var winnerAvatar = Games[gameId].Avatars.FirstOrDefault(x => !x.HasLeftTheGame);
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

                return;
            }

            // Regular game leave.
            HttpUtilities.FF(() =>
            {
                RequestManagerHttp.GameService.LeaveGame(gameId, connection.UserId);
            }, $"Error leaving the game with id {gameId}. UserId: {connection.UserId}");

            // 
            // 1. [API] Unregister user from the game
            // 2. [API] Lower the leaver MMR, IF he is not the winner!
            // 3. [API] Update the game state

            // 1. TCP call to let other players know that this player has left the game
            // 2. API call to update the game state and the user state
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
            return Games.FirstOrDefault(x => x.Value.Avatars.Any(y => y.UserId == userId)).Value.Id;
        }
    }
}
