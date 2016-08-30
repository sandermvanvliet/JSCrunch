using System;
using System.Linq;
using System.Reflection;
using JSCrunch.Core;
using Microsoft.Practices.Unity;
using Topshelf;

namespace JSCrunch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ApplicationDateTime.UtcNow = () => DateTime.UtcNow;

            var container = Bootstrapper.Boot();

            InitializeListeners(container);

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

        private static void InitializeListeners(IUnityContainer container)
        {
            Core.InitializeListeners.FromAssembly(container, typeof(ISubscribable).Assembly);
            Core.InitializeListeners.FromAssembly(container, typeof(Program).Assembly);
        }
    }
}