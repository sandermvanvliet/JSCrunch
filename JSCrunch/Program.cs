using System;
using System.Linq;
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
            var subscribable = typeof(ISubscribable);

            var subscribables = typeof(Program)
                .Assembly
                .GetTypes()
                .Where(type => type.IsAssignableFrom(subscribable))
                .ToList();

            var queue = container.Resolve<EventQueue>();

            foreach (var s in subscribables)
            {
                var instance = (ISubscribable)Activator.CreateInstance(s);

                queue.Subscribe(instance);

                container.RegisterInstance(instance);
            }
        }
    }
}