using System;
using System.Linq;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Metadata;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio.Listeners
{
    public class UpdateMedataListener : ISubscribable<UpdateMetadataEvent>
    {
        private readonly MetadataModel _model;
        private readonly EventQueue _eventQueue;

        public UpdateMedataListener(MetadataModel model, EventQueue eventQueue)
        {
            _model = model;
            _eventQueue = eventQueue;
        }

        public Type ForEventType => typeof(UpdateMetadataEvent);

        public void Publish(UpdateMetadataEvent eventInstance)
        {
            if (eventInstance is SolutionOpenedEvent)
            {
                var solutionOpenedEvent = (SolutionOpenedEvent)eventInstance;
                object pvar;
                solutionOpenedEvent.Solution.GetProperty((int)__VSPROPID.VSPROPID_SolutionBaseName, out pvar);
                var solutionName = (string)pvar;

                if (_model.SolutionName != solutionName)
                {
                    _model.SolutionName = solutionName;
                    _model.Projects.Clear();
                }
            }

            if (eventInstance is ProjectLoadedEvent)
            {
                var projectLoadedEvent = (ProjectLoadedEvent)eventInstance;
                
                var projectExists = _model.Projects.Any(c => c.Name == projectLoadedEvent.Project.GetProjectName());
                if (!projectExists)
                {
                    _model
                        .Projects
                        .Add(new ProjectModel { Name = projectLoadedEvent.Project.GetProjectName() });
                }
            }

            if (eventInstance is TestsFoundEvent)
            {
                var testsFoundEvent = (TestsFoundEvent)eventInstance;

                var project = _model.Projects.SingleOrDefault(c => c.Name == testsFoundEvent.ProjectName);
                if (project != null)
                {
                    foreach (var test in testsFoundEvent.Tests)
                    {
                        var exists = project.Tests.Any(c => c == test.Name);
                        if (!exists)
                        {
                            project
                                .Tests
                                .Add(test.Name);
                        }
                    }
                }
            }

            _eventQueue.Enqueue(new MetadataChangedEvent((MetadataModel)_model.Clone()));
        }
    }
}