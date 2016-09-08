using System;
using JSCrunch.Core;

namespace JSCrunch
{
    public interface IOutput
    {
        void Write(TestResult result);
        void Progress(string message);
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
                .ForEach(failedTest =>
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\t" + failedTest.Name);
                    Console.ResetColor();
                    Console.WriteLine("\t\t" + failedTest.Output);
                });
        }

        public void Progress(string message)
        {
            Console.WriteLine(message);
        }
    }
}