using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace JSCrunch.Core
{
    public static class InitializeListeners
    {
        public static void FromAssembly(IUnityContainer container, Assembly assembly)
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
                if (IsDeferredSubscribable(subscribable))
                {
                    continue;
                }

                container.RegisterType(subscribableType, subscribable, new ContainerControlledLifetimeManager());

                var instance = (ISubscribable)container.Resolve(subscribable);

                queue.Subscribe(instance);

                container.RegisterInstance(instance);
            }
        }

        private static bool IsDeferredSubscribable(Type subscribable)
        {
            var attribute = subscribable.GetCustomAttribute<SubscribableOptionsAttribute>();

            return attribute != null && attribute.LoadDeferred;
        }
    }
}