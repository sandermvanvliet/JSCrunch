using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio
{
    public class SolutionLoadedListener : ISubscribable
    {
        private readonly EventQueue _eventQueue;

        public SolutionLoadedListener(EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public Type ForEventType => typeof(SolutionLoadedEvent);

        public void Publish(Event eventInstance)
        {
            var solutionLoadedEvent = eventInstance as SolutionLoadedEvent;

            if (solutionLoadedEvent == null)
            {
                return;
            }

            var solution = solutionLoadedEvent.Solution;

            var webProjects = GetWebProjectsIn(solution);

            Debug.WriteLine("Found "+ webProjects.Count() + "projects");

            foreach (var webProject in webProjects)
            {
                _eventQueue.Enqueue(new DiscoverTestsEvent(webProject));
            }
        }

        private static IEnumerable<IVsProject> GetWebProjectsIn(IVsSolution solution)
        {
            IEnumHierarchies enumerator;
            var projects = new List<IVsProject>();

            if (solution.GetProjectEnum((uint) __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, Guid.Empty, out enumerator) == VSConstants.S_OK)
            {
                uint fetched;
                var vsHierarchies = new IVsHierarchy[1];

                enumerator.Next((uint)vsHierarchies.Length, vsHierarchies, out fetched);

                if (fetched > 0)
                {
                    projects.AddRange(vsHierarchies.Cast<IVsProject>().Take((int) fetched));
                }
            }

            return projects;
        }
    }
}