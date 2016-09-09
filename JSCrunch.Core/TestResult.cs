using System.Collections.Generic;
using System.Linq;

namespace JSCrunch.Core
{
    public class TestResult
    {
        public int NumberOfTests { get; set; }
        public int NumberOfFailures { get; set; }
        public List<TestCaseResult> FailedTests {
            get { return Tests.Where(t => !t.Success).ToList(); }
        }
        public int NumberPassed => NumberOfTests - NumberOfFailures;
        public string TestSuite { get; set; }
        public List<TestCaseResult> Tests { get; set; }
    }
}