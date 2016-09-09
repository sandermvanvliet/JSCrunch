using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio.Tests.Doubles
{
    public class IVsRunningDocumentTableDouble : IVsRunningDocumentTable
    {
        private string _file;

        public int RegisterAndLockDocument(uint grfRDTLockType, string pszMkDocument, IVsHierarchy pHier, uint itemid,
            IntPtr punkDocData, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        public int LockDocument(uint grfRDTLockType, uint dwCookie)
        {
            throw new NotImplementedException();
        }

        public int UnlockDocument(uint grfRDTLockType, uint dwCookie)
        {
            throw new NotImplementedException();
        }

        public int FindAndLockDocument(uint dwRDTLockType, string pszMkDocument, out IVsHierarchy ppHier, out uint pitemid,
            out IntPtr ppunkDocData, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        public int RenameDocument(string pszMkDocumentOld, string pszMkDocumentNew, IntPtr pHier, uint itemidNew)
        {
            throw new NotImplementedException();
        }

        public int AdviseRunningDocTableEvents(IVsRunningDocTableEvents pSink, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        public int UnadviseRunningDocTableEvents(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        public int GetDocumentInfo(uint docCookie, out uint pgrfRDTFlags, out uint pdwReadLocks, out uint pdwEditLocks,
            out string pbstrMkDocument, out IVsHierarchy ppHier, out uint pitemid, out IntPtr ppunkDocData)
        {
            pgrfRDTFlags = 0;
            pdwReadLocks = 0;
            pdwEditLocks = 0;
            pbstrMkDocument = _file;
            ppHier = null;
            pitemid = 0;
            ppunkDocData = IntPtr.Zero;

            return VSConstants.S_OK;
        }

        public int NotifyDocumentChanged(uint dwCookie, uint grfDocChanged)
        {
            throw new NotImplementedException();
        }

        public int NotifyOnAfterSave(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        public int GetRunningDocumentsEnum(out IEnumRunningDocuments ppenum)
        {
            throw new NotImplementedException();
        }

        public int SaveDocuments(uint grfSaveOpts, IVsHierarchy pHier, uint itemid, uint docCookie)
        {
            throw new NotImplementedException();
        }

        public int NotifyOnBeforeSave(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        public int RegisterDocumentLockHolder(uint grfRDLH, uint dwCookie, IVsDocumentLockHolder pLockHolder, out uint pdwLHCookie)
        {
            throw new NotImplementedException();
        }

        public int UnregisterDocumentLockHolder(uint dwLHCookie)
        {
            throw new NotImplementedException();
        }

        public int ModifyDocumentFlags(uint docCookie, uint grfFlags, int fSet)
        {
            throw new NotImplementedException();
        }

        public void GivenTheFile(string file)
        {
            _file = file;
        }
    }
}