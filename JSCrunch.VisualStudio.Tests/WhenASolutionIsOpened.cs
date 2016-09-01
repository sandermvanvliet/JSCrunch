using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenASolutionIsOpened
    {
        [TestMethod]
        public void ThenASolutionOpenedEventIsQueued()
        {
            var processingQueue = new List<ProcessingItem>();
            var eventQueue = new EventQueue();
            var handler = new VisualStudioEventHandler(processingQueue, eventQueue, Substitute.For<IServiceProvider>());

            handler.OnAfterOpenSolution(null, 0);

            eventQueue
                .OfType<SolutionOpenedEvent>()
                .Should()
                .HaveCount(1);
        }
    }
}