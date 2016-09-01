using System;
using System.Diagnostics;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio.Listeners
{
    public class ProjectLoadedListener : ISubscribable<ProjectLoadedEvent>
    {
        public Type ForEventType => typeof(ProjectLoadedEvent);
        
        public void Publish(ProjectLoadedEvent discoverTestsEvent)
        {
            Debug.WriteLine("Discovering tests in project " + discoverTestsEvent.Project.GetProjectName());
        }
    }
}