using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Listeners;
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

        [TestInitialize]
        public void Initialize()
        {
            _eventQueue = new EventQueue();
            _project = new VsProjectDouble();
            _listener = new ProjectLoadedListener(_eventQueue);
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
    }
}