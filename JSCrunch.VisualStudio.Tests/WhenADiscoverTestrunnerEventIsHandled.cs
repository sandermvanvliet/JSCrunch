using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenADiscoverTestrunnerEventIsHandled
    {
        [TestMethod]
        public void AndTheProjectDoesNotHaveTheChutzpahPackageThenNoTestRunnerAvailableEventIsQueued()
        {
            var project = new VsProjectDouble();
            var eventQueue = new EventQueue();
            var listener = new DiscoverTestrunnerListener(eventQueue, new VsPackageInstallerServicesDouble());

            listener.Publish(new DiscoverTestrunnerEvent(project));

            eventQueue
                .OfType<TestRunnerAvailableEvent>()
                .Should()
                .BeEmpty();
        }

        [TestMethod]
        public void AndTheProjectHasTheChutzpahPackageThenATestRunnerAvailableEventIsQueued()
        {
            var project = new VsProjectDouble();
            var eventQueue = new EventQueue();
            var packageInstallerDouble = new VsPackageInstallerServicesDouble();
            packageInstallerDouble.Add("Chutzpah", "Chutzpah runner", "c:\\temp\\TestProject\\packages\\chutzpah-1.0");
            var listener = new DiscoverTestrunnerListener(eventQueue, packageInstallerDouble);

            listener.Publish(new DiscoverTestrunnerEvent(project));

            eventQueue
                .OfType<TestRunnerAvailableEvent>()
                .Should()
                .HaveCount(1);
        }
    }
}