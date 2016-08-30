using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenASolutionLoadedEventIsQueued
    {
        [TestMethod]
        public void GivenASolutionWithAWebProjectThenADiscoverTestsEventIsQueued()
        {
            var eventQueue = new EventQueue();
            var solutionLoadedListener = new SolutionLoadedListener(eventQueue);

            var projects = new IVsProject[1];

            eventQueue.Subscribe(solutionLoadedListener);
            var solution = new VsSolutionDouble()
                .WithProjects(projects)
                .Build();

            eventQueue.Enqueue(new SolutionLoadedEvent(solution));

            eventQueue
                .OfType<DiscoverTestsEvent>()
                .Should()
                .HaveCount(1);
        }
    }
}