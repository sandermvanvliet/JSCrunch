using JSCrunch.Core.Events;

namespace JSCrunch.Core
{
    public interface ISubscribable
    {    
    }

    public interface ISubscribable<in TEvent> : ISubscribable where TEvent : Event
    {
        void Publish(TEvent eventInstance);
    }
}