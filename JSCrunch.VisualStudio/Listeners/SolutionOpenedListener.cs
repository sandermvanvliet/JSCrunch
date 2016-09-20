using JSCrunch.Core;
using JSCrunch.Core.Events;
using JSCrunch.VisualStudio.Events;

namespace JSCrunch.VisualStudio.Listeners
{
    public class SolutionOpenedListener : ISubscribable<SolutionOpenedEvent>
    {
        private readonly EventQueue _eventQueue;

        public SolutionOpenedListener(EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public void Publish(SolutionOpenedEvent eventInstance)
        {
        }
    }
}