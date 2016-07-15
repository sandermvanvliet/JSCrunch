using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class TestRunStartedEvent : Event
    {
        public string Path { get; }

        public TestRunStartedEvent(string path)
        {
            Path = path;
        }
    }
}