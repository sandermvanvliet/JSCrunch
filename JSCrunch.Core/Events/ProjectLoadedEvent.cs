using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.Core.Events
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