using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.Core.Tests
{
    [TestClass]
    public class WhenUnsubscribingFromTheEventQueue
    {
        [TestMethod]
        public void InvalidOperationExceptionIsThrownWhenListenerIsNotSubscribed()
        {
            var eventQueue = new EventQueue();
            var subscribable = new TestSubscribable();

            Action action = () => eventQueue.Unsubscribe(subscribable);

            action.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void ThenTheListenerDoesntReceiveNewEvents()
        {
            var eventQueue = new EventQueue();
            var subscribable = new TestSubscribable();
            eventQueue.Subscribe(subscribable);

            eventQueue.Unsubscribe(subscribable);
            eventQueue.Enqueue(new TestEvent());

            subscribable
                .ReceivedEvents
                .Should()
                .HaveCount(0);
        }
    }
}