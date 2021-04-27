using System.Collections.Generic;
using GameServer.Models;
using NetworkingShared.Enums;

namespace Assets.Scripts.Network.Services.HTTP.Interfaces
{
    public interface IGameService
    {
        Game GetGame(int gameId);

        Game CreateGame(GameParams gameConfig);

        Dictionary<CreatureType, UnitConfiguration> GetUnitConfigurations();
    }
}
