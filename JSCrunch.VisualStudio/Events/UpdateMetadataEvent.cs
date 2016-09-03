using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio.Events
{
    public class UpdateMetadataEvent : Event
    {
        public string ProjectName { get; set; }
    }
}