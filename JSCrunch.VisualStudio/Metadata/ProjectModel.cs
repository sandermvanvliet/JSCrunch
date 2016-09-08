using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JSCrunch.VisualStudio.Metadata
{
    public class ProjectModel
    {
        public ProjectModel()
        {
            Tests = new List<string>();
        }

        public List<string> Tests { get; set; }

        public string Name { get; set; }

        public ProjectModel Clone()
        {
            return new ProjectModel
            {
                Name = Name,
                Tests = Tests.Select(t => (string)t.Clone()).ToList()
            };
        }
    }
}