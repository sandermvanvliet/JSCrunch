using System;
using System.Collections.Generic;
using JSCrunch.Core.Events;

namespace JSCrunch.Core.Tests
{
    public class TestSubscribable : ISubscribable
    {
        public TestSubscribable()
        {
            ReceivedEvents = new List<Event>();
        }

        public List<Event> ReceivedEvents { get; }
        public Type ForEventType => typeof(TestEvent);

        public void Publish(Event eventInstance)
        {
            ReceivedEvents.Add(eventInstance);
        }
    }
}