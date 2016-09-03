using System.Collections.ObjectModel;

namespace JSCrunch.VisualStudio.Metadata
{
    public class MetadataModel
    {
        public MetadataModel()
        {
            Projects = new ObservableCollection<ProjectModel>();
        }

        public ObservableCollection<ProjectModel> Projects { get; }
    }
}