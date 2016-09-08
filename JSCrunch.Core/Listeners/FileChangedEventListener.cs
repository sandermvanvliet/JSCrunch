using System;
using System.Diagnostics;
using JSCrunch.Core.Events;

namespace JSCrunch.Core.Listeners
{
    public class FileChangedEventListener : ISubscribable<FileChangedEvent>
    {
        private readonly EventQueue _eventQueue;
        private readonly Configurator _configurator;

        public FileChangedEventListener(EventQueue eventQueue, Configurator configurator)
        {
            _eventQueue = eventQueue;
            _configurator = configurator;
        }

        public Type ForEventType => typeof(FileChangedEvent);

        public void Publish(FileChangedEvent fileChangedEvent)
        {
            InvokeTestRunnerOn(fileChangedEvent.Path);
        }

        private void InvokeTestRunnerOn(string path)
        {
            var arguments = string.Format(_configurator.TestRunnerParameters, path);

            _eventQueue.Enqueue(new TestRunStartedEvent(path));

            var process = ExecuteTestRunner(arguments);

            DumpResults(process.StartInfo.WorkingDirectory);

            _eventQueue.Enqueue(new TestRunCompletedEvent());
        }

        private Process ExecuteTestRunner(string arguments)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = _configurator.TestRunnerExecutable,
                    Arguments = arguments,
                    WorkingDirectory = Environment.CurrentDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            process.Start();

            try
            {
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to start test runner: " + ex.Message, ex);
            }
            return process;
        }

        private void DumpResults(string workingDirectory)
        {
            TestResultBuilder
                .From(workingDirectory)
                .ForEach(PublishTestResults);
        }

        private void PublishTestResults(TestResult testResult)
        {
            _eventQueue.Enqueue(new TestResultsAvailableEvent
            {
                FailedTests = testResult.FailedTests,
                NumberOfFailures = testResult.NumberOfFailures,
                NumberOfTests = testResult.NumberOfTests,
                TestSuite = testResult.TestSuite
            });
        }
    }
}