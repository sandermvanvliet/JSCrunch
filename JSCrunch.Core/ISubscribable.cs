using System;
using JSCrunch.Core.Events;

namespace JSCrunch.Core
{
    public interface ISubscribable
    {
        Type ForEventType { get; }
        void Publish(Event eventInstance);
    }
}