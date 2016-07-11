using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            _eventHandler = new VisualStudioEventHandler(_processingQueue);
        }

        [TestMethod]
        public void ThenNoEntryIsAddedToTheQueue()
        {
        }

        private void WhenTheFileIsSaved(string file)
        {
            _eventHandler.HandleDocumentSave(file);
        }

        private static string GivenAFile()
        {
            return @"c:\temp\foo.cs";
        }
    }
}