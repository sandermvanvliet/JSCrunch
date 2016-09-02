using System;
using System.Linq;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;

namespace JSCrunch.VisualStudio.Listeners
{
    public class DiscoverTestrunnerListener : ISubscribable<DiscoverTestrunnerEvent>
    {
        private readonly EventQueue _eventQueue;
        private readonly IVsPackageInstallerServices _packageInstaller;

        public DiscoverTestrunnerListener(EventQueue eventQueue, IVsPackageInstallerServices packageInstaller)
        {
            _eventQueue = eventQueue;
            _packageInstaller = packageInstaller;
        }

        public Type ForEventType => typeof(DiscoverTestrunnerEvent);

        public void Publish(DiscoverTestrunnerEvent eventInstance)
        {
            var envDteProject = ((IVsHierarchy)eventInstance.Project).GetEnvDteProject();

            var packages = _packageInstaller.GetInstalledPackages(envDteProject);

            var chutzpahPackage = packages.SingleOrDefault(p => p.Title.Contains("Chutzpah"));
            if (chutzpahPackage != null)
            {
                _eventQueue.Enqueue(new TestRunnerAvailableEvent(chutzpahPackage.InstallPath));
            }
        }
    }
}