using System;
using System.Linq;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Metadata;

namespace JSCrunch.VisualStudio.Listeners
{
    public class ProjectLoadedListener : ISubscribable<ProjectLoadedEvent>
    {
        private readonly EventQueue _eventQueue;
        private readonly MetadataModel _metadataModel;

        public ProjectLoadedListener(EventQueue eventQueue, MetadataModel metadataModel)
        {
            _eventQueue = eventQueue;
            _metadataModel = metadataModel;
        }

        public Type ForEventType => typeof(ProjectLoadedEvent);

        public void Publish(ProjectLoadedEvent projectLoadedEvent)
        {
            _eventQueue.Enqueue(new DiscoverTestsEvent(projectLoadedEvent.Project));
            _eventQueue.Enqueue(new DiscoverTestrunnerEvent(projectLoadedEvent.Project));

            _eventQueue.Enqueue(new UpdateMetadataEvent
            {
                ProjectName = projectLoadedEvent.Project.GetProjectName()
            });

            if (_metadataModel.Projects.All(p => p.Name != projectLoadedEvent.Project.GetProjectName()))
            {
                _metadataModel.Projects.Add(new ProjectModel { Name = projectLoadedEvent.Project.GetProjectName() });
            }
        }
    }
}