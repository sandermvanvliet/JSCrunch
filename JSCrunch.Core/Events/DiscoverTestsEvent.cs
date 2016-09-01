using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.Core.Events
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