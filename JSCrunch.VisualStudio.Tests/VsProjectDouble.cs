using System;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace JSCrunch.VisualStudio.Tests
{
    public class VsProjectDouble : IVsProject, IVsHierarchy
    {
        private readonly Dictionary<string, string> _files = new Dictionary<string, string>();
        private readonly Dictionary<__VSHPROPID, object> _properties = new Dictionary<__VSHPROPID, object>();

        public VsProjectDouble()
        {
            _properties.Add(__VSHPROPID.VSHPROPID_Name, "TestProject");
            _properties.Add(__VSHPROPID.VSHPROPID_ProjectDir, "c:\\temp\\TestProject");
            _properties.Add(__VSHPROPID.VSHPROPID_ExtObject, new EnvDteProjectDouble(this));
        }

        public int SetSite(IServiceProvider psp)
        {
            throw new NotImplementedException();
        }

        public int GetSite(out IServiceProvider ppSP)
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
            var property = (__VSHPROPID) propid;

            if (_properties.ContainsKey(property))
            {
                pvar = _properties[property];
                return VSConstants.S_OK;
            }

            pvar = null;
            return VSConstants.S_FALSE;
        }

        public int SetProperty(uint itemid, int propid, object var)
        {
            throw new NotImplementedException();
        }

        public int GetNestedHierarchy(uint itemid, ref Guid iidHierarchyNested, out IntPtr ppHierarchyNested,
            out uint pitemidNested)
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

        public int IsDocumentInProject(string pszMkDocument, out int pfFound, VSDOCUMENTPRIORITY[] pdwPriority,
            out uint pitemid)
        {
            if (_files.ContainsKey(pszMkDocument))
            {
                pfFound = 1;
                pitemid = 1;
            }
            else
            {
                pfFound = 0;
                pitemid = 0;
            }

            return VSConstants.S_OK;
        }

        public int GetMkDocument(uint itemid, out string pbstrMkDocument)
        {
            throw new NotImplementedException();
        }

        public int OpenItem(uint itemid, ref Guid rguidLogicalView, IntPtr punkDocDataExisting,
            out IVsWindowFrame ppWindowFrame)
        {
            throw new NotImplementedException();
        }

        public int GetItemContext(uint itemid, out IServiceProvider ppSP)
        {
            throw new NotImplementedException();
        }

        public int GenerateUniqueItemName(uint itemidLoc, string pszExt, string pszSuggestedRoot,
            out string pbstrItemName)
        {
            throw new NotImplementedException();
        }

        public int AddItem(uint itemidLoc, VSADDITEMOPERATION dwAddItemOperation, string pszItemName, uint cFilesToOpen,
            string[] rgpszFilesToOpen, IntPtr hwndDlgOwner, VSADDRESULT[] pResult)
        {
            throw new NotImplementedException();
        }

        public void AddFile(string filePath, string contents)
        {
            _files.Add(filePath, contents);
        }
    }
}