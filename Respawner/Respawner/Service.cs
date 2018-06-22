using log4net;
using Quartz;

namespace Respawner
{
    public class Service : IService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Service));
        private readonly IScheduler _scheduler;

        public Service(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Start()
        {
            _scheduler.Start();
        }

        public void Stop()
        {
            _scheduler.Shutdown();
        }
    }
}
