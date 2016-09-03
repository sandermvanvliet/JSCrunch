using System.Collections.ObjectModel;

namespace JSCrunch.VisualStudio.Metadata
{
    public class ProjectModel
    {
        public ProjectModel()
        {
            Tests = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Tests { get; set; }

        public string Name { get; set; }
    }
}