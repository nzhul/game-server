using System;
using System.Globalization;
using System.Threading.Tasks;
using log4net;
using Quartz;

namespace Respawner.Jobs
{
    [DisallowConcurrentExecution]
    public class RespawnJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Service));

        // IApiClient ...

        public RespawnJob() // inject DI here
        {
            // apiClient here
        }

        private async Task RespawnEntities()
        {
            log.Debug($"Running respawn routine at " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await RespawnEntities();
        }
    }
}
