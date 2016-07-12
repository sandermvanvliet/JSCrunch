using System;
using FluentAssertions;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.Core.Tests
{
    [TestClass]
    public class WhenSubscribingToTheEventQueue
    {
        [TestMethod]
        public void ThenAnSubscribeEventIsGenerated()
        {
            var eventQueue = new EventQueue();
            var subscribable = new TestSubscribable();

            eventQueue.Subscribe(subscribable);

            eventQueue
                .Peek()
                .Should()
                .BeOfType<ListenerSubscribedEvent>();
        }

        [TestMethod]
        public void GivenTheSameListenerIsRegisteredTwiceThenAnInvalidOperationExceptionIsThrown()
        {
            var eventQueue = new EventQueue();
            var subscribable = new TestSubscribable();

            eventQueue.Subscribe(subscribable);

            Action action = () => eventQueue.Subscribe(subscribable);

            action
                .ShouldThrow<InvalidOperationException>();
        }
    }
}