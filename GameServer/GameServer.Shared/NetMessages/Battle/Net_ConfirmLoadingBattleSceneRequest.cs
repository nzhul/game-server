using System;

namespace GameServer.Shared.NetMessages.Battle
{
    [Serializable]
    public class Net_ConfirmLoadingBattleSceneRequest : NetMessage
    {
        public Net_ConfirmLoadingBattleSceneRequest()
        {
            OperationCode = NetOperationCode.ConfirmLoadingBattleScene;
        }

        public Guid BattleId { get; set; }

        public int ArmyId { get; set; }

        public bool IsReady { get; set; }

        public bool IsValid()
        {
            bool result = true;

            if (this.BattleId == Guid.Empty)
            {
                return false;
            }

            if (this.ArmyId == 0)
            {
                return false;
            }

            if (!this.IsReady)
            {
                return false;
            }

            return result;
        }

    }
}