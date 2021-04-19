using System;
using GameServer.Shared.Models;

namespace GameServer.Shared.NetMessages.Battle
{
    [Serializable]
    public class Net_SwitchTurnEvent : NetMessage
    {
        public Net_SwitchTurnEvent()
        {
            this.OperationCode = NetOperationCode.OnSwitchTurn;
        }

        public Guid BattleId { get; set; }

        public int CurrentUnitId { get; set; }

        public Turn Turn { get; set; }
    }
}
