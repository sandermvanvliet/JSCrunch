using System.Collections.Generic;

namespace JSCrunch.VisualStudio.Metadata
{
    public class TestModel
    {
        public string Name { get; set; }
        public int NumberOfFailures { get; set; }
        public int PassedTests { get; set; }
        public List<ResultModel> Tests { get; set; }

        public TestModel Clone()
        {
            return new TestModel
            {
                Name = Name,
                NumberOfFailures = NumberOfFailures,
                PassedTests = PassedTests,
                Tests = Tests
            };
        }
    }
}