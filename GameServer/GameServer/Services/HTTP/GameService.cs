using System.Collections.Generic;
using Assets.Scripts.Network.Services.HTTP.Interfaces;
using GameServer.Models;
using NetworkingShared.Enums;

namespace Assets.Scripts.Network.Services.HTTP
{
    public class GameService : BaseService, IGameService
    {
        public Game GetGame(int gameId)
        {
            return base.Get<Game>($"games/{gameId}");
        }

        public Game CreateGame(GameParams gameConfig)
        {
            return base.Post<Game>($"games", gameConfig);
        }

        public Dictionary<CreatureType, UnitConfiguration> GetUnitConfigurations()
        {
            return base.Get<Dictionary<CreatureType, UnitConfiguration>>($"unit-configurations");
        }

        public void EndGame(int gameId, int winnerId)
        {
            base.Put($"games/{gameId}/{winnerId}/end");
        }

        public void LeaveGame(int gameId, int userId)
        {
            base.Put($"games/{gameId}/{userId}/leave");
        }
    }
}
