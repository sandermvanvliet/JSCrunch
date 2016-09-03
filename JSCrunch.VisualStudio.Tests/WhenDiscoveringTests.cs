using System.Linq;
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
        private EventQueue _eventQueue;

        [TestInitialize]
        public void Initialize()
        {
            _project = new VsProjectDouble();
            _project.AddFile("jscrunch.config", null);
            
            _eventQueue = new EventQueue();

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem
                .GetContentsOf(Arg.Any<string>())
                .Returns("<?xml version=\"1.0\"?><jscrunch><tests root=\"Tests\" pattern=\".*\\.Tests\\.ts\" /></jscrunch>");

            _listener = new DiscoverTestsListener(_eventQueue, fileSystem);
        }

        [TestMethod]
        public void AndAConfigFileExistsThenTheTestPatternIsLoaded()
        {
            _listener.Publish(new DiscoverTestsEvent(_project));

            _listener
                .ProjectConfiguration
                .TestPattern
                .Should()
                .Be(".*\\.Tests\\.ts");
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

        [TestMethod]
        public void AndTestsExistInTheRootThenATestsFoundEventIsQueued()
        {
            var envDteProject = _project.GetEnvDteProject();
            var testsFolder = envDteProject.ProjectItems.AddFolder("Tests");
            testsFolder.ProjectItems.AddFromFile("A.Tests.ts");
            testsFolder.ProjectItems.AddFromFile("B.Tests.ts");

            _listener.Publish(new DiscoverTestsEvent(_project));

            _eventQueue
                .OfType<TestsFoundEvent>()
                .Should()
                .HaveCount(1);
        }

        [TestMethod]
        public void AndTwoTestsExistInTheRootThenTheTestsFoundEventContainsTheTests()
        {
            var envDteProject = _project.GetEnvDteProject();
            var testsFolder = envDteProject.ProjectItems.AddFolder("Tests");
            testsFolder.ProjectItems.AddFromFile("A.Tests.ts");
            testsFolder.ProjectItems.AddFromFile("B.Tests.ts");

            _listener.Publish(new DiscoverTestsEvent(_project));

            _eventQueue
                .OfType<TestsFoundEvent>()
                .Single()
                .Tests
                .Should()
                .HaveCount(2);
        }

        [TestMethod]
        public void TheTestsFoundEventContainsTheProjectName()
        {
            var envDteProject = _project.GetEnvDteProject();
            var testsFolder = envDteProject.ProjectItems.AddFolder("Tests");
            testsFolder.ProjectItems.AddFromFile("A.Tests.ts");
            testsFolder.ProjectItems.AddFromFile("B.Tests.ts");

            _listener.Publish(new DiscoverTestsEvent(_project));

            _eventQueue
                .OfType<TestsFoundEvent>()
                .Single()
                .ProjectName
                .Should()
                .Be("TestProject");
        }
    }
}