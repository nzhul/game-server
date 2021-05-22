using System;
using GameServer.Matchmaking;

namespace GameServer.Scheduling.Jobs
{
    public class MatchmakingJob : JobBase
    {
        public MatchmakingJob()
            : base(new TimeSpan(0, 0, 1))
        {
        }

        protected override void DoWork()
        {
            //Console.WriteLine(DateTime.UtcNow.ToLongTimeString() + ": Perform matchmaking!");
            Matchmaker.Instance.DoMatchmaking();
        }
    }
}
