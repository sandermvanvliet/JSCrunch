using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;

namespace JSCrunch.VisualStudio.Listeners
{
    public class ProjectLoadedListener : ISubscribable<ProjectLoadedEvent>
    {
        private readonly EventQueue _eventQueue;

        public ProjectLoadedListener(EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public void Publish(ProjectLoadedEvent projectLoadedEvent)
        {
            _eventQueue.Enqueue(new DiscoverTestsEvent(projectLoadedEvent.Project));
            _eventQueue.Enqueue(new DiscoverTestrunnerEvent(projectLoadedEvent.Project));
        }
    }
}