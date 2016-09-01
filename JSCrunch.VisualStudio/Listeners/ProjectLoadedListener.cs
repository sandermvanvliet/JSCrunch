using System;
using System.Diagnostics;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio.Listeners
{
    public class ProjectLoadedListener : ISubscribable
    {
        public Type ForEventType => typeof(ProjectLoadedEvent);
        
        public void Publish(Event eventInstance)
        {
            var discoverTestsEvent = eventInstance as ProjectLoadedEvent;

            if (discoverTestsEvent == null)
            {
                return;
            }

            Debug.WriteLine("Discovering tests in project " + discoverTestsEvent.Project.GetProjectName());
        }
    }
}