using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Scheduling.Jobs;

namespace GameServer.Scheduling
{
    public class Scheduler
    {
        private static Scheduler _instance;

        public static Scheduler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Scheduler();
                }

                return _instance;
            }
        }

        // private constructor prevents all instantiations of this class other than the Singleton.
        private Scheduler() { }

        private IList<IJob> _jobs = new List<IJob>();

        public void Initialize()
        {
            var jobType = typeof(IJob);
            var jobs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => jobType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            foreach (var job in jobs)
            {
                var instance = (IJob)Activator.CreateInstance(job);
                _jobs.Add(instance);
            }
        }

        public void Tick()
        {
            foreach (var job in _jobs)
            {
                job.UpdateClock();
            }
        }
    }
}
