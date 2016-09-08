using System.Collections.Generic;
using JSCrunch.Core;

namespace JSCrunch.Tests
{
    public class OutputDouble : IOutput
    {
        public List<string> Messages { get; } = new List<string>();

        public void Write(TestResult result)
        {
            Messages.Add($"{result.TestSuite},{result.NumberOfTests},{result.NumberPassed},{result.NumberOfFailures}");
        }

        public void Progress(string message)
        {
            Messages.Add(message);
        }
    }
}