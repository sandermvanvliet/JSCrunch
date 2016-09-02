using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Listeners;
using JSCrunch.VisualStudio.Tests.Doubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenDiscoveringTests
    {
        private DiscoverTestsListener _listener;
        private VsProjectDouble _project;

        [TestInitialize]
        public void Initialize()
        {
            _project = new VsProjectDouble();
            _project.AddFile("jscrunch.config", null);

            var eventQueue = new EventQueue();

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem
                .GetContentsOf(Arg.Any<string>())
                .Returns("<?xml version=\"1.0\"?><jscrunch><tests root=\"Tests\" pattern=\"*.Tests.ts\" /></jscrunch>");

            _listener = new DiscoverTestsListener(eventQueue, fileSystem);
        }

        [TestMethod]
        public void AndAConfigFileExistsThenTheTestPatternIsLoaded()
        {
            _listener.Publish(new DiscoverTestsEvent(_project));

            _listener
                .ProjectConfiguration
                .TestPattern
                .Should()
                .Be("*.Tests.ts");
        }

        [TestMethod]
        public void AndAConfigFileExistsThenTheTestRootIsLoaded()
        {
            _listener.Publish(new DiscoverTestsEvent(_project));

            _listener
                .ProjectConfiguration
                .TestRoot
                .Should()
                .Be("Tests");
        }
    }
}