using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.Core.Events
{
    public class DiscoverTestsEvent : Event
    {
        private readonly IVsProject _webProject;

        public DiscoverTestsEvent(IVsProject webProject)
        {
            _webProject = webProject;
        }
    }
}