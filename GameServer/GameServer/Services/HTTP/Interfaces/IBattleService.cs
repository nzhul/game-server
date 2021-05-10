using System;

namespace Assets.Scripts.Network.Services.HTTP.Interfaces
{
    public interface IBattleService
    {
        void RegisterBattle(Guid battleId, int userId);

        void UnRegisterBattle(int userId);
    }
}
