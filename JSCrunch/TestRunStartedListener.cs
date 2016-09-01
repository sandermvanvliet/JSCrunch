using System;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class TestRunStartedListener : ISubscribable<TestRunStartedEvent>
    {
        private readonly IOutput _output;

        public TestRunStartedListener(IOutput output)
        {
            _output = output;
        }

        public Type ForEventType => typeof(TestRunStartedEvent);

        public void Publish(TestRunStartedEvent testRunStartedEvent)
        {
            _output.Progress($"Test run started for '{testRunStartedEvent.Path}'");
        }
    }
}