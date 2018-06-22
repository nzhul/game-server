using Autofac;
using log4net.Config;
using Respawner.Configuration;
using Topshelf;
using Topshelf.Autofac;

namespace Respawner
{
    static class Program
    {
        internal static void Main(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            XmlConfigurator.Configure();

            var container = new ContainerBuilder().ConfigureService();

            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.UseAutofacContainer(container)
                    .WithDetailsFromAppConfig(container)
                    .Service<IService>(configurator =>
                    {
                        configurator.ConstructUsingAutofacContainer()
                            .WhenStarted(service => { service.Start(); })
                            .WhenStopped(service =>
                            {
                                service.Stop();
                                container.Dispose();
                                container = null;
                            });
                    }).RunAsLocalSystem();
            });
        }
    }
}
