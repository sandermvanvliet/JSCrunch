using System.Collections.ObjectModel;

namespace JSCrunch.VisualStudio.Metadata
{
    public class ProjectModel
    {
        public ObservableCollection<string> Tests { get; set; }
        public string Name { get; set; }
    }
}