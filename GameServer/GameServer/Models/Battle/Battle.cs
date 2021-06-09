﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Models.Units;
using GameServer.Utilities;

namespace GameServer.Models.Battle
{
    public class Battle
    {
        public Battle()
        {
            this.Start = DateTime.UtcNow;
            this.Log = new List<string>();
            this.Armies = new List<Army>();
        }

        public Guid Id { get; set; }

        public int GameId { get; set; }

        public int CurrentArmyId { get; set; } // Replace of Turn enum

        public Unit CurrentUnit { get; set; }

        public BattleState State { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime LastTurnStartTime { get; set; }

        public List<string> Log { get; set; }

        public List<Army> Armies { get; set; }

        public void UpdateLastActivity(int armyId)
        {
            Armies.Find(x => x.Id == armyId).LastActivity = DateTime.UtcNow;
        }

        public Army SwitchTurn()
        {
            var currentArmy = Armies.Find(x => x.Id == CurrentArmyId);
            currentArmy.TurnConsumed = true;
            var availableArmies = Armies.Where(x => !x.TurnConsumed);

            if (availableArmies.Count() == 0)
            {
                Armies.ForEach(x => x.TurnConsumed = false);
            }

            var nextArmy = Armies.OrderByDescending(x => x.TurnOrder).First();
            CurrentArmyId = nextArmy.Id;

            CurrentUnit = GetRandomAvailibleUnit(nextArmy);

            return nextArmy;

        }

        public Unit GetRandomAvailibleUnit(Army army)
        {
            var availibleUnits = army.Units.Where(x => !x.ActionConsumed).ToList();
            return availibleUnits[RandomGenerator.RandomNumber(0, army.Units.Count - 1)]; // TODO: Not tested
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
