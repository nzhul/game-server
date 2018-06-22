using System.Configuration;

namespace Respawner.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public string Description => ConfigurationManager.AppSettings["Service.Description"];
        public string ServiceName => ConfigurationManager.AppSettings["Service.ServiceName"];
        public string DisplayName => ConfigurationManager.AppSettings["Service.DisplayName"];
    }
}