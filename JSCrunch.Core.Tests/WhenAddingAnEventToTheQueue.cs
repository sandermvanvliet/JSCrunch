using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.Core.Tests
{
    [TestClass]
    public class WhenAddingAnEventToTheQueue
    {
        [TestMethod]
        public void ThenTheCounterReflectsTheNumberOfEvents()
        {
            var eventQueue = new EventQueue();

            eventQueue.Enqueue(new TestEvent());

            eventQueue
                .Count
                .Should()
                .Be(1);
        }

        [TestMethod]
        public void ThenSubscribedListenerForEventTypeReceivesEvent()
        {
            var eventQueue = new EventQueue();
            var subscribable = new TestSubscribable();
            eventQueue.Subscribe(subscribable);

            eventQueue.Enqueue(new TestEvent());

            subscribable
                .ReceivedEvents
                .Should()
                .HaveCount(1);
        }
    }
}