using System;
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
        private readonly IServiceProvider _visualStudioServiceProvider;

        public VisualStudioEventHandler(List<ProcessingItem> processingQueue, EventQueue eventQueue, IServiceProvider visualStudioServiceProvider)
        {
            _processingQueue = processingQueue;
            _eventQueue = eventQueue;
            _visualStudioServiceProvider = visualStudioServiceProvider;
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
            _eventQueue.Enqueue(new DiscoverTestsEvent(null));

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
            var currentSolution = _visualStudioServiceProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            
            _eventQueue.Enqueue(new SolutionLoadedEvent(currentSolution));

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