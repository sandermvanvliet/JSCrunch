using System.Collections.Generic;
using JSCrunch.Core;

namespace JSCrunch.VisualStudio
{
    public class VisualStudioEventHandler
    {
        private readonly List<ProcessingItem> _processingQueue;

        public VisualStudioEventHandler(List<ProcessingItem> processingQueue)
        {
            _processingQueue = processingQueue;
        }

        public void HandleDocumentSave(string file)
        {
            _processingQueue.Add(new ProcessingItem { FileName = file, Timestamp = ApplicationDateTime.UtcNow() });
        }
    }
}