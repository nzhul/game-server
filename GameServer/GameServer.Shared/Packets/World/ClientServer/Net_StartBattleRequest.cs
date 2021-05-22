using System;
using GameServer.Shared.Models;

namespace GameServer.Shared.Packets.World.ClientServer
{
    [Serializable]
    public class Net_StartBattleRequest : NetMessage
    {
        public Net_StartBattleRequest()
        {
            OperationCode = NetOperationCode.StartBattleRequest;
        }

        public int AttackerArmyId { get; set; }

        public int DefenderArmyId { get; set; }

        public PlayerType AttackerType { get; set; }

        public PlayerType DefenderType { get; set; }

        public bool IsValid()
        {
            bool result = true;

            if (this.AttackerArmyId == 0)
            {
                return false;
            }

            if (this.DefenderArmyId == 0)
            {
                return false;
            }

            return result;
        }
    }
}
