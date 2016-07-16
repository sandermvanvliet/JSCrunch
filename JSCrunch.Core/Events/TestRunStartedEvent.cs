namespace JSCrunch.Core.Events
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