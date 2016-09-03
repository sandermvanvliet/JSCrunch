using System.Collections.Generic;
using EnvDTE;
using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio.Events
{
    public class TestsFoundEvent : Event
    {
        public IEnumerable<ProjectItem> Tests { get; }
        public string ProjectName { get; }

        public TestsFoundEvent(IEnumerable<ProjectItem> tests, string projectName)
        {
            Tests = tests;
            ProjectName = projectName;
        }
    }
}