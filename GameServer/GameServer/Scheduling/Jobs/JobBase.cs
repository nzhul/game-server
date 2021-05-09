using System;

namespace GameServer.Scheduling.Jobs
{
    public abstract class JobBase : IJob
    {
        public JobBase(TimeSpan tickInterval)
        {
            _tickInterval = tickInterval;
        }

        protected TimeSpan _tickInterval;

        protected DateTime _lastExecutionTime;

        public void UpdateClock()
        {
            if (_lastExecutionTime + _tickInterval < DateTime.UtcNow)
            {
                this.DoWork();
                _lastExecutionTime = DateTime.UtcNow;
            }
        }

        protected abstract void DoWork();
    }
}
