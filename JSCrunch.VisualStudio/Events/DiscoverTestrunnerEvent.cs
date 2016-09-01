using JSCrunch.Core.Events;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio.Events
{
    public class DiscoverTestrunnerEvent : Event
    {
        public IVsProject Project { get; }

        public DiscoverTestrunnerEvent(IVsProject project)
        {
            Project = project;
        }
    }
}