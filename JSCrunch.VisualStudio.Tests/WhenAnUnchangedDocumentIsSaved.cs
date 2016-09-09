using System;
using System.Collections.Generic;
using JSCrunch.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenAnUnchangedDocumentIsSaved
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
        public void ThenNoEntryIsAddedToTheQueue()
        {
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