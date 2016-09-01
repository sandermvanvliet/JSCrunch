using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using JSCrunch.Core;
using Microsoft.Practices.Unity;

namespace JSCrunch.VisualStudio
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize(IServiceProvider visualStudioServiceProvider)
        {
            var container = new UnityContainer();

            var eventQueue = new EventQueue();
            container.RegisterInstance(typeof(EventQueue), eventQueue);

            container.RegisterInstance(typeof(IServiceProvider), visualStudioServiceProvider);

            var processingQueue = new List<ProcessingItem>();
            container.RegisterInstance(typeof(List<ProcessingItem>), processingQueue);

            ((IServiceContainer)visualStudioServiceProvider).AddService(typeof(IUnityContainer), container);

            return container;
        }
    }
}