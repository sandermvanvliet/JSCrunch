using System;
using EnvDTE;

namespace JSCrunch.VisualStudio.Tests
{
    public class EnvDteProjectDouble : EnvDTE.Project
    {
        private readonly VsProjectDouble _vsProjectDouble;

        public EnvDteProjectDouble(VsProjectDouble vsProjectDouble)
        {
            _vsProjectDouble = vsProjectDouble;
        }

        public void SaveAs(string NewFileName)
        {
            throw new NotImplementedException();
        }

        public void Save(string FileName = "")
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public string FileName { get; }
        public bool IsDirty { get; set; }
        public Projects Collection { get; }
        public DTE DTE { get; }
        public string Kind { get; }
        public ProjectItems ProjectItems { get; }
        public Properties Properties { get; }
        public string UniqueName { get; }
        public object Object { get; }
        public object get_Extender(string ExtenderName)
        {
            throw new NotImplementedException();
        }

        public object ExtenderNames { get; }
        public string ExtenderCATID { get; }
        public string FullName { get; }
        public bool Saved { get; set; }
        public ConfigurationManager ConfigurationManager { get; }
        public Globals Globals { get; }
        public ProjectItem ParentProjectItem { get; }
        public CodeModel CodeModel { get; }
    }
}