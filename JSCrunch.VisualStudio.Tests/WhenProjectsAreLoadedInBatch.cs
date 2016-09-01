using System;
using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenProjectsAreLoadedInBatch
    {
        [TestMethod]
        public void ThenADiscoverTestsEventIsQueuedForLoadedProjects()
        {
            var eventQueue = new EventQueue();
            var solutionDouble = new VsSolutionDouble();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(SVsSolution)).Returns(solutionDouble);
            var eventHandler = new VisualStudioEventHandler(null, eventQueue, serviceProvider);

            // Pretend we have one project loaded
            solutionDouble.WithProjects(new [] { new VsProjectDouble() });

            eventHandler.OnAfterLoadProjectBatch(true);

            eventQueue
                .OfType<DiscoverTestsEvent>()
                .Should()
                .HaveCount(1);
        }
    }
}