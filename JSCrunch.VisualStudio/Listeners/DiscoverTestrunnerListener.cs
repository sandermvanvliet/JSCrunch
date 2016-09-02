using System;
using System.Linq;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;

namespace JSCrunch.VisualStudio.Listeners
{
    public class DiscoverTestrunnerListener : ISubscribable<DiscoverTestrunnerEvent>
    {
        private readonly EventQueue _eventQueue;
        private readonly IServiceProvider _serviceProvider;

        public DiscoverTestrunnerListener(EventQueue eventQueue, IServiceProvider serviceProvider)
        {
            _eventQueue = eventQueue;
            _serviceProvider = serviceProvider;
        }

        public Type ForEventType => typeof(DiscoverTestrunnerEvent);

        public void Publish(DiscoverTestrunnerEvent eventInstance)
        {
            var envDteProject = ((IVsHierarchy)eventInstance.Project).GetEnvDteProject();

            var componentModel = (IComponentModel)_serviceProvider.GetService(typeof(SComponentModel));
            var packageInstallerServices = componentModel.GetService<IVsPackageInstallerServices>();
           
            var packages = packageInstallerServices.GetInstalledPackages(envDteProject);

            var chutzpahPackage = packages.SingleOrDefault(p => p.Id.Contains("Chutzpah"));
            if (chutzpahPackage != null)
            {
                _eventQueue.Enqueue(new TestRunnerAvailableEvent(chutzpahPackage.InstallPath));
            }
        }
    }
}