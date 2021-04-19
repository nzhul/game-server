using GameServer.Shared.Models;

namespace Assets.Scripts.Network.Services.TCP.Interfaces
{
    public interface IBattleService
    {
        void SwitchTurn(Battle battle);

        void NullifyUnitPoints(int gameId, int heroId, int unitId, bool isDefend);

        void EndBattle(Battle battle, int winnerId);
    }
}
