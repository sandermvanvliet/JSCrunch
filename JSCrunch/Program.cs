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
            InitializeListenersFromAssembly(container, typeof(ISubscribable).Assembly);
            InitializeListenersFromAssembly(container, typeof(Program).Assembly);
        }

        private static void InitializeListenersFromAssembly(IUnityContainer container, Assembly assembly)
        {
            var subscribableType = typeof(ISubscribable);

            var implementingTypes = assembly
                .GetTypes()
                .Where(type => subscribableType.IsAssignableFrom(type) &&
                               type.IsClass &&
                               !type.IsAbstract)
                .ToList();

            var queue = container.Resolve<EventQueue>();

            foreach (var subscribable in implementingTypes)
            {
                container.RegisterType(subscribableType, subscribable, new ContainerControlledLifetimeManager());

                var instance = (ISubscribable)container.Resolve(subscribable);

                queue.Subscribe(instance);

                container.RegisterInstance(instance);
            }
        }
    }
}