using System;
using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Listeners;
using JSCrunch.VisualStudio.Tests.Doubles;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NuGet.VisualStudio;

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
            var serviceProvider = Substitute.For<IServiceProvider>();
            var componentModel = Substitute.For<IComponentModel>();
            serviceProvider.GetService(Arg.Is(typeof(SComponentModel))).Returns(componentModel);
            componentModel.GetService<IVsPackageInstallerServices>().Returns(new VsPackageInstallerServicesDouble());
            var listener = new DiscoverTestrunnerListener(eventQueue, serviceProvider);

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
            packageInstallerDouble.Add("Chutzpah", "4.2.3", "Chutzpah Description",
                "c:\\temp\\TestProject\\packages\\chutzpah-1.0");

            var componentModel = Substitute.For<IComponentModel>();
            componentModel.GetService<IVsPackageInstallerServices>().Returns(packageInstallerDouble);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(SComponentModel)).ReturnsForAnyArgs(componentModel);

            var listener = new DiscoverTestrunnerListener(eventQueue, serviceProvider);

            listener.Publish(new DiscoverTestrunnerEvent(project));

            eventQueue
                .OfType<TestRunnerAvailableEvent>()
                .Should()
                .HaveCount(1);
        }

        [TestMethod]
        public void AndTheProjectHasTheChutzpahPackageThenTheTestRunnerAvailableEventContainsThePathToTheTestRunner()
        {
            var project = new VsProjectDouble();
            var eventQueue = new EventQueue();

            var packageInstallerDouble = new VsPackageInstallerServicesDouble();
            packageInstallerDouble.Add("Chutzpah", "4.2.3", "Chutzpah Description",
                "c:\\temp\\TestProject\\packages\\chutzpah-1.0");

            var componentModel = Substitute.For<IComponentModel>();
            componentModel.GetService<IVsPackageInstallerServices>().Returns(packageInstallerDouble);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(SComponentModel)).ReturnsForAnyArgs(componentModel);

            var listener = new DiscoverTestrunnerListener(eventQueue, serviceProvider);

            listener.Publish(new DiscoverTestrunnerEvent(project));

            eventQueue
                .OfType<TestRunnerAvailableEvent>()
                .Single()
                .TestRunnerPath
                .Should()
                .Be("c:\\temp\\TestProject\\packages\\chutzpah-1.0\\tools\\chutzpah.console.exe");
        }
    }
}