using System;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Metadata;

namespace JSCrunch.VisualStudio.Listeners
{
    public class MetadataRequestedListener : ISubscribable<MetadataRequestedEvent>
    {
        private readonly EventQueue _eventQueue;
        private readonly MetadataModel _metadataModel;

        public MetadataRequestedListener(EventQueue eventQueue, MetadataModel metadataModel)
        {
            _eventQueue = eventQueue;
            _metadataModel = metadataModel;
        }

        public Type ForEventType => typeof(MetadataRequestedEvent);

        public void Publish(MetadataRequestedEvent eventInstance)
        {
            _eventQueue.Enqueue(new MetadataChangedEvent((MetadataModel)_metadataModel.Clone()));
        }
    }
}