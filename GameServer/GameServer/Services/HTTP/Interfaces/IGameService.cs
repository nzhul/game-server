using System.Collections.Generic;
using GameServer.Shared.Models;

namespace Assets.Scripts.Network.Services.HTTP.Interfaces
{
    public interface IGameService
    {
        Game GetGame(int gameId);

        Game CreateGame(GameParams gameConfig);

        Dictionary<CreatureType, UnitConfiguration> GetUnitConfigurations();
    }
}
