using System;
using System.Diagnostics;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio
{
    public class DiscoverTestsListener : ISubscribable
    {
        public Type ForEventType => typeof(DiscoverTestsEvent);
        
        public void Publish(Event eventInstance)
        {
            var discoverTestsEvent = eventInstance as DiscoverTestsEvent;

            if (discoverTestsEvent == null)
            {
                return;
            }

            Debug.WriteLine("Discovering tests in project " + discoverTestsEvent.Project.GetProjectName());
        }
    }
}