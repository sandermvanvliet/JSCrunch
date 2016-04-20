using System;
using Topshelf;

namespace JSCrunch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ApplicationDateTime.UtcNow = () => DateTime.UtcNow;

            var container = Bootstrapper.Boot();

            HostFactory
                .Run(configurator =>
                {
                    configurator.Service<WatcherService>(serviceConfigurator =>
                    {
                        serviceConfigurator.ConstructUsing(() => container.Resolve<WatcherService>());
                        serviceConfigurator.WhenStarted(watcherService => watcherService.Start());
                        serviceConfigurator.WhenStopped(watcherService => watcherService.Stop());
                    });

                    configurator.RunAsLocalService();

                    configurator.SetDescription("Watches for changes on files in a directory to execute tests");
                    configurator.SetDisplayName("JSCrunch Service");
                    configurator.SetServiceName("JSCrunchService");
                });
        }
    }
}