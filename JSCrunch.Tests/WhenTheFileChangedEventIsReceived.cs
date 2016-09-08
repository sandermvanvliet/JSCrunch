using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using JSCrunch.Core.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.Tests
{
    [TestClass]
    public class WhenTheFileChangedEventIsReceived
    {
        private EventQueue _eventQueue;
        private FileChangedEventListener _listener;
        private readonly string _workingDirectorty = Path.GetTempPath();

        [TestInitialize]
        public void Initialize()
        {
            _eventQueue = new EventQueue();
            _listener = new FileChangedEventListener(_eventQueue, new DummyConfigurator());
        }

        [TestMethod]
        public void ThenATestRunStartedEventIsPublished()
        {
            AFileChanged();

            _eventQueue
                .OfType<TestRunStartedEvent>()
                .Should()
                .HaveCount(1, "there should be a TestRunStartedEvent");
        }

        [TestMethod]
        public void AndTheTestRunCompletesThenATestRunCompletedEventIsPublished()
        {
            AFileChanged();

            _eventQueue
                .OfType<TestRunCompletedEvent>()
                .Should()
                .HaveCount(1);
        }

        [TestMethod]
        public void AndThereAreTestResultsThenATestResultsAvailableEventIsPublished()
        {
            var resultsFile = Path.Combine(Environment.CurrentDirectory, "results.xml");
            File.WriteAllText(resultsFile, "<?xml version=\"1.0\" encoding=\"utf-8\"?><testsuites><testsuite name=\"test\" tests=\"1\" failures=\"0\"></testsuite></testsuites>");
            AFileChanged();

            _eventQueue
                .OfType<TestResultsAvailableEvent>()
                .Should()
                .HaveCount(1);
        }

        private void AFileChanged()
        {
            _listener.Publish(new FileChangedEvent(Path.Combine(_workingDirectorty, "somefile.js")));
        }
    }
}