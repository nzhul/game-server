using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Managers;
using GameServer.Models.Battle;

namespace GameServer.Scheduling.Jobs
{
    public class SwitchBattleTurnsJob : JobBase
    {
        private readonly TimeSpan TURN_DURATION = new TimeSpan(0, 0, 20); // seconds
        private readonly TimeSpan IDLE_TIMEOUT = new TimeSpan(0, 5, 50); // Use this for testing. Real one is bellow!
        //private const int IDLE_TIMEOUT = (TURN_DURATION * 2) + (TURN_DURATION / 2); // seconds -> 20 * 2 + 20 / 2 = 40 + 10 = 50

        public SwitchBattleTurnsJob()
            : base(new TimeSpan(0, 0, 1))
        {
        }

        protected override void DoWork()
        {
            var activeBattles = BattleManager.Instance.GetBattles();
            var completedBattles = new List<Battle>();

            foreach (var battle in activeBattles)
            {
                if (battle.State != BattleState.Fight)
                {
                    continue;
                }

                // 1. Check if both players are inactive.
                DateTime idleTime = DateTime.UtcNow - IDLE_TIMEOUT;
                if (battle.Armies.All(x => x.LastActivity < idleTime))
                {
                    Console.WriteLine($"Ending Idle battle: BattleId: {battle.Id}");
                    BattleManager.Instance.EndBattle(battle, -1);
                    completedBattles.Add(battle);
                }

                if (battle.LastTurnStartTime + TURN_DURATION < DateTime.UtcNow)
                {
                    BattleManager.Instance.SwitchTurn(battle);
                }
            }

            foreach (var battle in completedBattles)
            {
                BattleManager.Instance.UnRegisterBattle(battle);
            }
        }
    }
}
