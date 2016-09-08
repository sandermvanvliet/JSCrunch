using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JSCrunch.VisualStudio.Metadata
{
    public class ProjectModel
    {
        public ProjectModel()
        {
            Tests = new List<TestModel>();
        }

        public List<TestModel> Tests { get; set; }

        public string Name { get; set; }

        public ProjectModel Clone()
        {
            return new ProjectModel
            {
                Name = Name,
                Tests = Tests.Select(t => t.Clone()).ToList()
            };
        }
    }
}