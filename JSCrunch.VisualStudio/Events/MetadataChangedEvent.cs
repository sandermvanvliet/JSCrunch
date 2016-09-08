using JSCrunch.Core.Events;
using JSCrunch.VisualStudio.Metadata;

namespace JSCrunch.VisualStudio.Events
{
    public class MetadataChangedEvent : Event
    {
        public MetadataModel Model { get; }

        public MetadataChangedEvent(MetadataModel model)
        {
            Model = model;
        }
    }
}