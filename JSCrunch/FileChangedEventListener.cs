using System;
using System.Diagnostics;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class FileChangedEventListener : ISubscribable
    {
        private readonly EventQueue _eventQueue;
        private readonly Configurator _configurator;

        public FileChangedEventListener(EventQueue eventQueue, Configurator configurator)
        {
            _eventQueue = eventQueue;
            _configurator = configurator;
        }

        public Type ForEventType => typeof(FileChangedEvent);

        public void Publish(Event eventInstance)
        {
            var fileChangedEvent = eventInstance as FileChangedEvent;
            if (fileChangedEvent != null)
            {
                InvokeTestRunnerOn(fileChangedEvent.Path);
            }
        }

        private void InvokeTestRunnerOn(string path)
        {
            var arguments = string.Format(_configurator.TestRunnerParameters, path);

            _eventQueue.Enqueue(new TestRunStartedEvent(path));

            var process = ExecuteTestRunner(arguments);

            DumpResults(process.StartInfo.WorkingDirectory);
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
            TestResults
                .From(workingDirectory)
                .ForEach(PublishTestResultEvent);
        }

        private void PublishTestResultEvent(TestResult testResult)
        {
            _eventQueue.Enqueue(new TestRunCompletedEvent
            {
                FailedTests = testResult.FailedTests,
                NumberOfFailures = testResult.NumberOfFailures,
                NumberOfTests = testResult.NumberOfTests,
                TestSuite = testResult.TestSuite
            });
        }
    }
}