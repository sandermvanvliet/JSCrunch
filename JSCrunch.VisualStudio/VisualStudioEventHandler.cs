using System.Collections.Generic;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio
{
    public class VisualStudioEventHandler : IVsSolutionEvents
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

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            _eventQueue.Enqueue(new SolutionLoadedEvent(null));

            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }
    }
}