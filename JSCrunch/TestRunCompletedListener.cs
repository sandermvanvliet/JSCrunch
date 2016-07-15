using System;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class TestRunCompletedListener : ISubscribable
    {
        private readonly IOutput _output;

        public TestRunCompletedListener(IOutput output)
        {
            _output = output;
        }

        public Type ForEventType => typeof(TestRunCompletedEvent);

        public void Publish(Event eventInstance)
        {
            var testRunCompletedEvent = (TestRunCompletedEvent) eventInstance;
            if (testRunCompletedEvent != null)
            {
                _output.Write(new TestResult
                {
                    FailedTests = testRunCompletedEvent.FailedTests,
                    NumberOfFailures = testRunCompletedEvent.NumberOfFailures,
                    NumberOfTests = testRunCompletedEvent.NumberOfTests,
                    TestSuite = testRunCompletedEvent.TestSuite
                });
            }
        }
    }
}