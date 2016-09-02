using JSCrunch.Core.Events;

namespace JSCrunch.VisualStudio.Events
{
    public class TestRunnerAvailableEvent : Event
    {
        public string TestRunnerPath { get; }

        public TestRunnerAvailableEvent(string testRunnerPath)
        {
            TestRunnerPath = testRunnerPath;
        }
    }
}