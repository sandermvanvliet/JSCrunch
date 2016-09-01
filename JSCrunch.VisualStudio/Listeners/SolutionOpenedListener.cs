using System;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio.Listeners
{
    public class SolutionOpenedListener : ISubscribable
    {
        private readonly EventQueue _eventQueue;

        public SolutionOpenedListener(EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public Type ForEventType => typeof(SolutionOpenedEvent);

        public void Publish(Event eventInstance)
        {
            var solutionLoadedEvent = eventInstance as SolutionOpenedEvent;

            if (solutionLoadedEvent == null)
            {
            }
        }
    }
}