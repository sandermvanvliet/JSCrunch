using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class TestRunCompletedListener : ISubscribable<TestResultsAvailableEvent>
    {
        private readonly IOutput _output;

        public TestRunCompletedListener(IOutput output)
        {
            _output = output;
        }

        public void Publish(TestResultsAvailableEvent testRunCompletedEvent)
        {
            _output.Write(new TestResult
            {
                Tests = testRunCompletedEvent.Tests,
                NumberOfFailures = testRunCompletedEvent.NumberOfFailures,
                NumberOfTests = testRunCompletedEvent.NumberOfTests,
                TestSuite = testRunCompletedEvent.TestSuite
            });
        }
    }
}