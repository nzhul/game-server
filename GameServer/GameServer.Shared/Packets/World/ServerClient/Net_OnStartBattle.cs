using System;
using GameServer.Shared.Models;

namespace GameServer.Shared.Packets.World.ServerClient
{
    [Serializable]
    public class Net_OnStartBattle : NetMessage
    {
        public Net_OnStartBattle()
        {
            this.OperationCode = NetOperationCode.OnStartBattle;
        }

        public Guid BattleId { get; set; }

        public int AttackerArmyId { get; set; }

        public int DefenderArmyId { get; set; }

        public int SelectedUnitId { get; set; }

        public PlayerType AttackerType { get; set; }

        public PlayerType DefenderType { get; set; }

        public BattleScenario BattleScenario { get; set; }

        public Turn Turn { get; set; }
    }
}
