using JSCrunch.Core.Events;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio.Events
{
    public class DiscoverTestsEvent : Event
    {
        public IVsProject Project { get; }

        public DiscoverTestsEvent(IVsProject project)
        {
            Project = project;
        }
    }
}