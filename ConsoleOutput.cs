using System;

namespace JSCrunch
{
    public interface IOutput
    {
        void Write(TestResult result);
    }

    public class ConsoleOutput : IOutput
    {
        public void Write(TestResult result)
        {
            var message =
                        $"[{result.TestSuite}] {result.NumberOfTests} total, {result.NumberOfFailures} failures, {result.NumberPassed} passed";

            var foregroundColor = result.NumberOfFailures == 0 ? ConsoleColor.Green : ConsoleColor.Red;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            Console.ResetColor();

            result
                .FailedTests
                .ForEach(failedTest => Console.WriteLine("\t" + failedTest));
        }
    }
}