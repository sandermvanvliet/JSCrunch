using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JSCrunch.Core.Events;

namespace JSCrunch.Core
{
    public class EventQueue : IEnumerable<Event>
    {
        private readonly List<Event> _queue;
        private readonly List<ISubscribable> _listeners;

        public EventQueue()
        {
            _queue = new List<Event>();
            _listeners = new List<ISubscribable>();
        }

        public void Enqueue<TEvent>(TEvent eventInstance) where TEvent: Event
        {
            _queue.Add(eventInstance);

            var listenersForEvent = _listeners
                .OfType<ISubscribable<TEvent>>()
                .ToList();

            foreach (var listener in listenersForEvent)
            {
                listener.Publish(eventInstance);
            }
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

        public void Unsubscribe(ISubscribable subscribable)
        {
            if (!_listeners.Contains(subscribable))
            {
                throw new InvalidOperationException();
            }

            _listeners.Remove(subscribable);
        }

        public IEnumerator<Event> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}