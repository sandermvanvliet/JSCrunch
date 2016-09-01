using JSCrunch.Core.Events;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio.Events
{
    public class ProjectLoadedEvent : Event
    {
        public IVsProject Project { get; }

        public ProjectLoadedEvent(IVsProject project)
        {
            Project = project;
        }
    }
}