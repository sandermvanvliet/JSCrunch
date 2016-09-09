using System.Collections.Generic;
using JSCrunch.Core;

namespace JSCrunch.VisualStudio.Metadata
{
    public class TestModel
    {
        public TestModel Clone()
        {
            return new TestModel
            {
                Name = Name,
                NumberOfFailures = NumberOfFailures,
                PassedTests = PassedTests,
                FailedTests = FailedTests
            };
        }

        public string Name { get; set; }
        public int NumberOfFailures { get; set; }
        public int PassedTests { get; set; }
        public List<TestCaseResult> FailedTests { get; set; }
    }
}