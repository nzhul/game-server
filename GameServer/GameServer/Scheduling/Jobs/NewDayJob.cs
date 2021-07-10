using System;
using GameServer.Managers;

namespace GameServer.Scheduling.Jobs
{
    public class NewDayJob : JobBase
    {
        public NewDayJob()
            : base(new TimeSpan(0, 0, 1))
        {
        }

        protected override void DoWork()
        {
            GameManager.Instance.DoNewDayTimeCheck(_tickInterval);
        }
    }
}
