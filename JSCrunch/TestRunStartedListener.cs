using System;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class TestRunStartedListener : ISubscribable
    {
        private readonly IOutput _output;

        public TestRunStartedListener(IOutput output)
        {
            _output = output;
        }

        public Type ForEventType => typeof(TestRunStartedEvent);

        public void Publish(Event eventInstance)
        {
            var testRunStartedEvent = eventInstance as TestRunStartedEvent;

            if (testRunStartedEvent != null)
            {
                _output.Progress($"Test run started for '{testRunStartedEvent.Path}'");
            }
        }
    }
}