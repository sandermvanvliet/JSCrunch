using System.Collections.Generic;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class TestRunCompletedEvent : Event
    {
        public int NumberOfTests { get; set; }
        public int NumberOfFailures { get; set; }
        public List<TestResults.TestCaseResult> FailedTests { get; set; }
        public int NumberPassed => NumberOfTests - NumberOfFailures;
        public string TestSuite { get; set; }
    }
}