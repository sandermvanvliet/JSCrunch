using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Metadata;
using Microsoft.Practices.Unity;

namespace JSCrunch.VisualStudio
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize(IServiceProvider visualStudioServiceProvider)
        {
            var container = new UnityContainer();

            container.RegisterInstance(typeof(EventQueue), new EventQueue());

            container.RegisterInstance(typeof(IServiceProvider), visualStudioServiceProvider);

            container.RegisterInstance(typeof(List<ProcessingItem>), new List<ProcessingItem>());

            container.RegisterType<IFileSystem, FileSystem>();

            container.RegisterType<MetadataModel, MetadataModel>(new ContainerControlledLifetimeManager());

            ((IServiceContainer)visualStudioServiceProvider).AddService(typeof(IUnityContainer), container);

            return container;
        }
    }
}