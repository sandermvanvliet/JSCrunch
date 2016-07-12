using System;
using System.Collections.Generic;
using System.Linq;
using JSCrunch.Core.Events;

namespace JSCrunch.Core
{
    public class EventQueue
    {
        private readonly List<Event> _queue;
        private readonly List<ISubscribable> _listeners;

        public EventQueue()
        {
            _queue = new List<Event>();
            _listeners = new List<ISubscribable>();
        }

        public void Enqueue(Event eventInstance)
        {
            _queue.Add(eventInstance);
        }

        public long Count => _queue.Count;

        public void Subscribe(ISubscribable subscribable)
        {
            if (_listeners.Contains(subscribable))
            {
                throw new InvalidOperationException();
            }

            _listeners.Add(subscribable);

            Enqueue(new ListenerSubscribedEvent());
        }

        public Event Peek()
        {
            return _queue.Last();
        }
    }
}