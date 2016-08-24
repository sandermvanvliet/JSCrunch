using System.Collections.Generic;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio
{
    public class VisualStudioEventHandler
    {
        private readonly List<ProcessingItem> _processingQueue;
        private readonly EventQueue _eventQueue;

        public VisualStudioEventHandler(List<ProcessingItem> processingQueue, EventQueue eventQueue)
        {
            _processingQueue = processingQueue;
            _eventQueue = eventQueue;
        }

        public void HandleDocumentSave(string file)
        {
            _processingQueue.Add(new ProcessingItem
            {
                FileName = file,
                Timestamp = ApplicationDateTime.UtcNow()
            });
        }

        public void HandleSolutionLoaded()
        {
            _eventQueue.Enqueue(new SolutionLoadedEvent());
        }
    }
}