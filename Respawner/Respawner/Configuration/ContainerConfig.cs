using Autofac;
using Autofac.Extras.Quartz;

namespace Respawner.Configuration
{
    public static class ContainerConfig
    {
        public static IContainer ConfigureService(this ContainerBuilder builder)
        {
            return builder
                .RegisterServices()
                .RegisterQuartz()
                .Build();
        }

        private static ContainerBuilder RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterType<Service>().As<IService>();
            builder.RegisterType<ServiceConfiguration>().As<IServiceConfiguration>();

            // register apiClient here

            return builder;
        }

        private static ContainerBuilder RegisterQuartz(this ContainerBuilder builder)
        {
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(IService).Assembly));

            return builder;
        }
    }
}
