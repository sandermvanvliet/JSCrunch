using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    public class WatcherService : ISubscribable
    {
        private readonly Configurator _configurator;
        private readonly EventQueue _eventQueue;
        private readonly object _syncRoot = new object();
        private readonly Queue<TestRequest> _testRequests;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _isRunning;

        public WatcherService(Configurator configurator, EventQueue eventQueue)
        {
            _configurator = configurator;
            _eventQueue = eventQueue;
            _testRequests = new Queue<TestRequest>();
            _configurator.UpdatedSettingsAvailable += HandleUpdatedSettingsAvailable;

            InitializeFileWatcher(configurator);

            TestRequestQueued += HandleTestRequestQueued;
        }

        private event EventHandler<EventArgs> TestRequestQueued;

        private void HandleTestRequestQueued(object sender, EventArgs e)
        {
            RunTests();
        }

        private void RunTests()
        {
            lock (_syncRoot)
            {
                if (_isRunning)
                {
                    return;
                }

                _isRunning = true;
            }

            while (_testRequests.Any())
            {
                var requestToRun = _testRequests.Dequeue();

                InvokeTestRunnerOn(requestToRun.Path);
            }

            lock (_syncRoot)
            {
                _isRunning = false;
            }
        }

        private void InvokeTestRunnerOn(string path)
        {
            var arguments = string.Format(_configurator.TestRunnerParameters, path);

            _eventQueue.Enqueue(new TestRunStartedEvent(path));

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

                DumpResults(process.StartInfo.WorkingDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to start test runner: " + ex.Message, ex);
            }
        }

        public void DumpResults(string workingDirectory)
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

        private void HandleUpdatedSettingsAvailable(object sender, EventArgs e)
        {
            InitializeFileWatcher(_configurator);
        }

        private void InitializeFileWatcher(Configurator configurator)
        {
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
                _fileSystemWatcher.Dispose();
                _fileSystemWatcher = null;
            }

            _fileSystemWatcher = new FileSystemWatcher(configurator.PathToWatch, configurator.TestPattern);
            _fileSystemWatcher.IncludeSubdirectories = configurator.IncludeSubdirectories;
            _fileSystemWatcher.Changed += HandleFileChanged;
            _fileSystemWatcher.Created += HandleFileChanged;
            _fileSystemWatcher.Renamed += HandleFileRenamed;
        }

        private void HandleFileRenamed(object sender, RenamedEventArgs e)
        {
            EnqueueTestRequest(e.FullPath);
        }

        private void EnqueueTestRequest(string path)
        {
            _eventQueue.Enqueue(new FileChangedEvent(path));
        }

        private void HandleFileChanged(object sender, FileSystemEventArgs e)
        {
            EnqueueTestRequest(e.FullPath);
        }

        public void Start()
        {
            _eventQueue.Subscribe(this);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            _eventQueue.Unsubscribe(this);
        }

        protected void OnTestRequestQueued()
        {
            TestRequestQueued?.BeginInvoke(this, EventArgs.Empty, null, null);
        }

        public Type ForEventType => typeof(FileChangedEvent);

        public void Publish(Event eventInstance)
        {
            var fileChangedEvent = (FileChangedEvent) eventInstance;
            _testRequests.Enqueue(new TestRequest(eventInstance.Timestamp, fileChangedEvent.Path));
            OnTestRequestQueued();
        }
    }
}