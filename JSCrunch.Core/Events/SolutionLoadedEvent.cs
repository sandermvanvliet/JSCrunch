using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.Core.Events
{
    public class SolutionLoadedEvent : JSCrunch.Core.Events.Event
    {
        public SolutionLoadedEvent(IVsSolution solution)
        {
            Solution = solution;
        }

        public IVsSolution Solution { get; private set; }
    }
}