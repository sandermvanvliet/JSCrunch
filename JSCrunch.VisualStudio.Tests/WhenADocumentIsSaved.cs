using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using JSCrunch.VisualStudio.Tests.Doubles;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenADocumentIsSaved
    {
        private VisualStudioEventHandler _eventHandler;
        private IVsRunningDocumentTableDouble _runningDocumentsTable;
        private EventQueue _eventQueue;

        [TestInitialize]
        public void Initialize()
        {
            var visualStudioServiceProvider = Substitute.For<IServiceProvider>();
            _runningDocumentsTable = new IVsRunningDocumentTableDouble();
            visualStudioServiceProvider.GetService(typeof(SVsRunningDocumentTable)).Returns(_runningDocumentsTable);
            _eventQueue = new EventQueue();
            _eventHandler = new VisualStudioEventHandler(_eventQueue, visualStudioServiceProvider);
        }

        [TestMethod]
        public void ThenAnEntryIsAddedToTheQueue()
        {
            var file = GivenAFile();

            WhenTheFileIsSaved(file);

            _eventQueue
                .OfType<FileChangedEvent>()
                .Should()
                .HaveCount(1);
        }

        [TestMethod]
        public void ThenTheProcessingEntryContainsTheFileName()
        {
            var filePath = GivenAFile();

            WhenTheFileIsSaved(filePath);

            _eventQueue
                .OfType<FileChangedEvent>()
                .Single()
                .Path
                .Should()
                .Be(filePath);
        }

        [TestMethod]
        public void ThenTheProcessingEntryContainsTheTimestamp()
        {
            var expectedTimestamp = DateTime.UtcNow;
            ApplicationDateTime.UtcNow = () => expectedTimestamp;
            var filePath = GivenAFile();

            WhenTheFileIsSaved(filePath);

            _eventQueue
                .OfType<FileChangedEvent>()
                .Single()
                .Timestamp
                .Should()
                .Be(ApplicationDateTime.UtcNow());
        }

        [TestMethod]
        public void WhenTwoSaveEventsAreHandledThenTwoProcessingEntriesAreAdded()
        {
            var filePath = GivenAFile();

            WhenTheFileIsSaved(filePath);
            WhenTheFileIsSaved(filePath);

            _eventQueue
                .OfType<FileChangedEvent>()
                .Should()
                .HaveCount(2);
        }

        private void WhenTheFileIsSaved(string file)
        {
            _runningDocumentsTable.GivenTheFile(file);
            _eventHandler.OnAfterSave(DocCookieOf(file));
        }

        private uint DocCookieOf(string file)
        {
            return 1;
        }

        private static string GivenAFile()
        {
            return @"c:\temp\foo.cs";
        }
    }
}