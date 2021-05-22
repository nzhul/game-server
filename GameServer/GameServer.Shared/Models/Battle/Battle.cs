using System;
using System.Collections.Generic;
using GameServer.Shared.Models.Units;

namespace GameServer.Shared.Models
{
    public class Battle
    {
        public Battle()
        {
            this.Start = DateTime.UtcNow;
            this.AttackerLastActivity = DateTime.UtcNow;
            this.DefenderLastActivity = DateTime.UtcNow;
            this.Log = new List<string>();
        }

        public Guid Id { get; set; }

        public int GameId { get; set; }

        public int AttackerArmyId { get; set; }

        public int AttackerConnectionId { get; set; }

        public Army AttackerArmy { get; set; }

        public DateTime AttackerLastActivity { get; set; }

        public bool AttackerDisconnected { get; set; }

        public int DefenderArmyId { get; set; }

        public int DefenderConnectionId { get; set; }

        public Army DefenderArmy { get; set; }

        public DateTime DefenderLastActivity { get; set; }

        public bool DefenderDisconnected { get; set; }

        public Unit SelectedUnit { get; set; }

        public PlayerType AttackerType { get; set; }

        public PlayerType DefenderType { get; set; }

        public bool AttackerReady { get; set; }

        public bool DefenderReady { get; set; }

        public BattleState State { get; set; }

        public BattleScenario BattleScenario { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public float LastTurnStartTime { get; set; }

        public Turn Turn { get; set; }

        public List<string> Log { get; set; }

        public void UpdateLastActivity(int heroId)
        {
            if (this.AttackerArmyId == heroId)
            {
                this.AttackerLastActivity = DateTime.UtcNow;
            }
            else if (this.DefenderArmyId == heroId)
            {
                this.DefenderLastActivity = DateTime.UtcNow;
            }
        }

        // Attacker Troops

        // Defender Troops

        // BATTLE FLOW
        // 1. Battle is registered in server scheduler
        // 2. Battle is in pause state on both clients
        // 3. Scheduler sends SwapTurn message to the clients by giving the first turn to the attacker
        // 4. Scheduler track turn start time and compares whether it is > than 75 seconds every 5 seconds.
        // 5. If true -> sends another swap message
    }
}
