using System;
using System.Collections.Generic;
using System.Linq;

namespace JSCrunch.VisualStudio.Metadata
{
    public class MetadataModel : ICloneable
    {
        public MetadataModel()
        {
            Projects = new List<ProjectModel>();
        }

        public string SolutionName { get; set; }

        public List<ProjectModel> Projects { get; set; }

        public object Clone()
        {
            var clone = new MetadataModel();

            clone.SolutionName = SolutionName;
            clone.Projects = Projects.Select(p => p.Clone()).ToList();

            return clone;
        }
    }
}