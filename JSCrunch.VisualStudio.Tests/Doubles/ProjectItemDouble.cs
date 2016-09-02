using System;
using EnvDTE;

namespace JSCrunch.VisualStudio.Tests.Doubles
{
    public class ProjectItemDouble : ProjectItem
    {
        private readonly ProjectItems _projectItems = new ProjectItemsDouble();

        public ProjectItemDouble(string name)
        {
            Name = name;
        }

        public bool SaveAs(string NewFileName)
        {
            throw new NotImplementedException();
        }

        public Window Open(string ViewKind = "{00000000-0000-0000-0000-000000000000}")
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void ExpandView()
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

        public bool IsDirty { get; set; }
        public string get_FileNames(short index)
        {
            throw new NotImplementedException();
        }

        public short FileCount { get; }
        public string Name { get; set; }
        public ProjectItems Collection => _projectItems;
        public Properties Properties { get; }
        public DTE DTE { get; }
        public string Kind { get; }
        public ProjectItems ProjectItems => _projectItems;
        public bool get_IsOpen(string ViewKind = "{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}")
        {
            throw new NotImplementedException();
        }

        public object Object { get; }
        public object get_Extender(string ExtenderName)
        {
            throw new NotImplementedException();
        }

        public object ExtenderNames { get; }
        public string ExtenderCATID { get; }
        public bool Saved { get; set; }
        public ConfigurationManager ConfigurationManager { get; }
        public FileCodeModel FileCodeModel { get; }
        public Document Document { get; }
        public Project SubProject { get; }
        public Project ContainingProject { get; }
    }
}