using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Listeners;
using JSCrunch.VisualStudio.Metadata;
using JSCrunch.VisualStudio.Tests.Doubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenAProjectLoadedEventIsHandled
    {
        private EventQueue _eventQueue;
        private ProjectLoadedListener _listener;
        private VsProjectDouble _project;
        private MetadataModel _metadataModel;

        [TestInitialize]
        public void Initialize()
        {
            _metadataModel = new MetadataModel();
            _eventQueue = new EventQueue();
            _project = new VsProjectDouble();
            _listener = new ProjectLoadedListener(_eventQueue, _metadataModel);
        }

        [TestMethod]
        public void ThenADiscoverTestsEventIsQueued()
        {
            _listener.Publish(new ProjectLoadedEvent(_project));

            _eventQueue
                .OfType<DiscoverTestsEvent>()
                .Should()
                .HaveCount(1);
        }

        [TestMethod]
        public void ThenADiscoverTestrunnerEventIsQueued()
        {
            _listener.Publish(new ProjectLoadedEvent(_project));

            _eventQueue
                .OfType<DiscoverTestrunnerEvent>()
                .Should()
                .HaveCount(1);
        }

        [TestMethod]
        public void ThenAnUpdateMetadataEventIsQueued()
        {
            _listener.Publish(new ProjectLoadedEvent(_project));

            _eventQueue
                .OfType<UpdateMetadataEvent>()
                .Should()
                .HaveCount(1);
        }

        [TestMethod]
        public void TheUpdateMetadataEventContainsTheProjectName()
        {
            _listener.Publish(new ProjectLoadedEvent(_project));

            _eventQueue
                .OfType<UpdateMetadataEvent>()
                .Single()
                .ProjectName
                .Should()
                .Be("TestProject");
        }

        [TestMethod]
        public void AndTheProjectIsNotInTheMetadataModelThenItIsAdded()
        {
            _listener.Publish(new ProjectLoadedEvent(_project));

            _metadataModel.Projects.Select(p => p.Name).Should().ContainSingle(_project.GetProjectName());
        }

        [TestMethod]
        public void AndTheProjectIsInTheMetadataModelThenItIsntAdded()
        {
            _metadataModel.Projects.Add(new ProjectModel { Name = _project.GetProjectName() });

            _listener.Publish(new ProjectLoadedEvent(_project));

            _metadataModel.Projects.Should().HaveCount(1);
        }
    }
}