using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenADocumentIsSaved
    {
        private List<ProcessingItem> _processingQueue;
        private VisualStudioEventHandler _eventHandler;
         
        [TestInitialize]
        public void Initialize()
        {
            _processingQueue = new List<ProcessingItem>();
            _eventHandler = new VisualStudioEventHandler(new EventQueue(), Substitute.For<IServiceProvider>());
        }

        [TestMethod]
        public void ThenAnEntryIsAddedToTheQueue()
        {
            var file = GivenAFile();

            WhenTheFileIsSaved(file);

            _processingQueue
                .Count
                .Should()
                .Be(1);
        }

        [TestMethod]
        public void ThenTheProcessingEntryContainsTheFileName()
        {
            var filePath = GivenAFile();

            WhenTheFileIsSaved(filePath);

            _processingQueue
                .Single()
                .FileName
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

            _processingQueue
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

            _processingQueue
                .Count
                .Should()
                .Be(2);
        }

        private void WhenTheFileIsSaved(string file)
        {
        }

        private static string GivenAFile()
        {
            return @"c:\temp\foo.cs";
        }
    }
}