using System;
using System.Collections;
using System.Collections.Generic;
using EnvDTE;

namespace JSCrunch.VisualStudio.Tests.Doubles
{
    public class ProjectItemsDouble : ProjectItems
    {
        private readonly List<ProjectItem> _projectItems = new List<ProjectItem>();

        public ProjectItem Item(object index)
        {
            throw new NotImplementedException();
        }

        IEnumerator ProjectItems.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public ProjectItem AddFromFile(string FileName)
        {
            var projectItem = new ProjectItemDouble(FileName);
            _projectItems.Add(projectItem);
            return projectItem;
        }

        public ProjectItem AddFromTemplate(string FileName, string Name)
        {
            throw new NotImplementedException();
        }

        public ProjectItem AddFromDirectory(string Directory)
        {
            throw new NotImplementedException();
        }

        public ProjectItem AddFolder(string Name, string Kind = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}")
        {
            var projectItem = new ProjectItemDouble(Name);
            _projectItems.Add(projectItem);
            return projectItem;
        }

        public ProjectItem AddFromFileCopy(string FilePath)
        {
            throw new NotImplementedException();
        }

        public object Parent { get; }
        public int Count { get; }
        public DTE DTE { get; }
        public string Kind { get; }
        public Project ContainingProject { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _projectItems.GetEnumerator();
        }
    }
}