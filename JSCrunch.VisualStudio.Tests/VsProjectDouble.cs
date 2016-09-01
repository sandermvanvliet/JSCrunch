using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio.Tests
{
    public class VsProjectDouble : IVsProject, IVsHierarchy
    {
        public int IsDocumentInProject(string pszMkDocument, out int pfFound, VSDOCUMENTPRIORITY[] pdwPriority, out uint pitemid)
        {
            throw new NotImplementedException();
        }

        public int GetMkDocument(uint itemid, out string pbstrMkDocument)
        {
            throw new NotImplementedException();
        }

        public int OpenItem(uint itemid, ref Guid rguidLogicalView, IntPtr punkDocDataExisting, out IVsWindowFrame ppWindowFrame)
        {
            throw new NotImplementedException();
        }

        public int GetItemContext(uint itemid, out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
        {
            throw new NotImplementedException();
        }

        public int GenerateUniqueItemName(uint itemidLoc, string pszExt, string pszSuggestedRoot, out string pbstrItemName)
        {
            throw new NotImplementedException();
        }

        public int AddItem(uint itemidLoc, VSADDITEMOPERATION dwAddItemOperation, string pszItemName, uint cFilesToOpen,
            string[] rgpszFilesToOpen, IntPtr hwndDlgOwner, VSADDRESULT[] pResult)
        {
            throw new NotImplementedException();
        }

        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            throw new NotImplementedException();
        }

        public int GetSite(out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
        {
            throw new NotImplementedException();
        }

        public int QueryClose(out int pfCanClose)
        {
            throw new NotImplementedException();
        }

        public int Close()
        {
            throw new NotImplementedException();
        }

        public int GetGuidProperty(uint itemid, int propid, out Guid pguid)
        {
            throw new NotImplementedException();
        }

        public int SetGuidProperty(uint itemid, int propid, ref Guid rguid)
        {
            throw new NotImplementedException();
        }

        public int GetProperty(uint itemid, int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        public int SetProperty(uint itemid, int propid, object var)
        {
            throw new NotImplementedException();
        }

        public int GetNestedHierarchy(uint itemid, ref Guid iidHierarchyNested, out IntPtr ppHierarchyNested, out uint pitemidNested)
        {
            throw new NotImplementedException();
        }

        public int GetCanonicalName(uint itemid, out string pbstrName)
        {
            throw new NotImplementedException();
        }

        public int ParseCanonicalName(string pszName, out uint pitemid)
        {
            throw new NotImplementedException();
        }

        public int Unused0()
        {
            throw new NotImplementedException();
        }

        public int AdviseHierarchyEvents(IVsHierarchyEvents pEventSink, out uint pdwCookie)
        {
            throw new NotImplementedException();
        }

        public int UnadviseHierarchyEvents(uint dwCookie)
        {
            throw new NotImplementedException();
        }

        public int Unused1()
        {
            throw new NotImplementedException();
        }

        public int Unused2()
        {
            throw new NotImplementedException();
        }

        public int Unused3()
        {
            throw new NotImplementedException();
        }

        public int Unused4()
        {
            throw new NotImplementedException();
        }
    }
}