using System;
using System.Collections.Generic;
using JSCrunch.Core.Events;

namespace JSCrunch.Core.Tests
{
    public class TestSubscribable : ISubscribable<TestEvent>
    {
        public TestSubscribable()
        {
            ReceivedEvents = new List<Event>();
        }

        public List<Event> ReceivedEvents { get; }
        public Type ForEventType => typeof(TestEvent);

        public void Publish(TestEvent eventInstance)
        {
            ReceivedEvents.Add(eventInstance);
        }
    }
}