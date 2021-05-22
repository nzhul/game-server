using System;
using Assets.Scripts.Network.Services.HTTP.Interfaces;

namespace Assets.Scripts.Network.Services.HTTP
{
    public class BattleService : BaseService, IBattleService
    {
        public void RegisterBattle(Guid battleId, int userId)
        {
            base.Put($"battles/{battleId}/register/{userId}");
        }

        public void UnRegisterBattle(int userId)
        {
            base.Put($"battles/unregister/{userId}");
        }
    }
}
