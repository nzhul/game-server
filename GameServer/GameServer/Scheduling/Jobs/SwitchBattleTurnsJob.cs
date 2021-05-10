using System;
using GameServer.Managers;
using GameServer.Models.Battle;

namespace GameServer.Scheduling.Jobs
{
    public class SwitchBattleTurnsJob : JobBase
    {
        private readonly TimeSpan TURN_DURATION = new TimeSpan(0, 0, 20); // seconds
        private readonly TimeSpan IDLE_TIMEOUT = new TimeSpan(0, 0, 10000); // Use this for testing. Real one is bellow!
        //private const int IDLE_TIMEOUT = (TURN_DURATION * 2) + (TURN_DURATION / 2); // seconds -> 20 * 2 + 20 / 2 = 40 + 10 = 50

        public SwitchBattleTurnsJob()
            : base(new TimeSpan(0, 0, 1))
        {
        }

        protected override void DoWork()
        {
            var activeBattles = BattleManager.Instance.GetBattles();
            foreach (var battle in activeBattles)
            {
                if (battle.State != BattleState.Fight)
                {
                    continue;
                }

                // 1. Check if both players are inactive.
                DateTime idleTime = DateTime.UtcNow - IDLE_TIMEOUT;
                if (battle.AttackerLastActivity < idleTime && battle.DefenderLastActivity < idleTime)
                {
                    Console.WriteLine($"Ending Idle battle: AttackerId: {battle.AttackerArmy.Id}, Defender: {battle.DefenderArmy.Id}, BattleId: {battle.Id}");
                    BattleManager.Instance.EndBattle(battle, -1);
                }

                if (battle.LastTurnStartTime + TURN_DURATION < DateTime.UtcNow)
                {
                    BattleManager.Instance.SwitchTurn(battle);
                }
            }
        }
    }
}
