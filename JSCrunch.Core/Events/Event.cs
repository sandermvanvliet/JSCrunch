using System;

namespace JSCrunch.Core.Events
{
    public abstract class Event
    {
        protected Event()
        {
            Timestamp = ApplicationDateTime.UtcNow();
        }

        public DateTime Timestamp { get; private set; }
        public string Name => GetType().Name;
    }
}