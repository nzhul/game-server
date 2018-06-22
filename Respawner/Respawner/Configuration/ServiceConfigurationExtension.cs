using Autofac;
using Topshelf.HostConfigurators;

namespace Respawner.Configuration
{
    internal static class ServiceConfigurationExtension
    {
        public static HostConfigurator WithDetailsFromAppConfig(this HostConfigurator configurator, IContainer container)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var configuration = scope.Resolve<IServiceConfiguration>();
                configurator.SetDescription(configuration.Description);
                configurator.SetDisplayName(configuration.DisplayName);
                configurator.SetServiceName(configuration.ServiceName);
            }

            return configurator;
        }
    }
}
