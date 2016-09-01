using System;
using System.Diagnostics;
using JSCrunch.Core;
using JSCrunch.Core.Events;
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

        public Type ForEventType => typeof(ProjectLoadedEvent);
        
        public void Publish(ProjectLoadedEvent projectLoadedEvent)
        {
            Debug.WriteLine("Discovering tests in project " + projectLoadedEvent.Project.GetProjectName());

            _eventQueue.Enqueue(new DiscoverTestsEvent());
            _eventQueue.Enqueue(new DiscoverTestrunnerEvent());
        }
    }
}