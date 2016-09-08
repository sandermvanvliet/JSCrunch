using System.Collections.Generic;

namespace JSCrunch.Core
{
    public class TestResult
    {
        public int NumberOfTests { get; set; }
        public int NumberOfFailures { get; set; }
        public List<TestCaseResult> FailedTests { get; set; }
        public int NumberPassed => NumberOfTests - NumberOfFailures;
        public string TestSuite { get; set; }
    }
}