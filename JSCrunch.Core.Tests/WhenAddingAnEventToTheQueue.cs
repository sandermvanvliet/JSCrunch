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
    }
}