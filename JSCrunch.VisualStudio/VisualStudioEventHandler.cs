using System;
using System.Diagnostics;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using JSCrunch.VisualStudio.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio
{
    public class VisualStudioEventHandler : IVsSolutionEvents, IVsSolutionLoadEvents, IVsRunningDocTableEvents
    {
        private readonly EventQueue _eventQueue;
        private readonly IServiceProvider _visualStudioServiceProvider;
        private IVsRunningDocumentTable _runningDocumentsTable;

        public VisualStudioEventHandler(EventQueue eventQueue, IServiceProvider visualStudioServiceProvider)
        {
            _eventQueue = eventQueue;
            _visualStudioServiceProvider = visualStudioServiceProvider;
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            object name;
            pHierarchy.GetProperty((uint)VSConstants.VSITEMID.Nil, (int)__VSHPROPID.VSHPROPID_Name, out name);
            Debug.WriteLine("OnAfterOpenProject: " + name);
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
            _eventQueue.Enqueue(new ProjectLoadedEvent(pRealHierarchy as IVsProject));

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
            
            _eventQueue.Enqueue(new SolutionOpenedEvent(currentSolution));

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

        public int OnBeforeOpenSolution(string pszSolutionFilename)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeBackgroundSolutionLoadBegins()
        {
            return VSConstants.S_OK;
        }

        public int OnQueryBackgroundLoadProjectBatch(out bool pfShouldDelayLoadToNextIdle)
        {
            pfShouldDelayLoadToNextIdle = false;

            return VSConstants.S_OK;
        }

        public int OnBeforeLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProjectBatch(bool fIsBackgroundIdleBatch)
        {
            var solution = _visualStudioServiceProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            var projects = solution.GetLoadedProjects();

            foreach (var project in projects)
            {
                _eventQueue.Enqueue(new ProjectLoadedEvent(project));
            }

            return VSConstants.S_OK;
        }

        public int OnAfterBackgroundSolutionLoadComplete()
        {
            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            if (_runningDocumentsTable == null)
            {
                _runningDocumentsTable = _visualStudioServiceProvider.GetService(typeof(SVsRunningDocumentTable)) as IVsRunningDocumentTable;
            }

            uint rdtFlags;
            uint readLocks;
            uint editLocks;
            string mkDocument;
            IVsHierarchy hierarchy;
            uint itemId;
            IntPtr ppunkDocData;

            if (_runningDocumentsTable.GetDocumentInfo(docCookie, out rdtFlags, out readLocks, out editLocks,
                out mkDocument, out hierarchy, out itemId, out ppunkDocData) == VSConstants.S_OK)
            {
                _eventQueue.Enqueue(new FileChangedEvent(mkDocument));
            }

            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }
    }
}