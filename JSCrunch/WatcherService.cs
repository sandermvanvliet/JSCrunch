using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace JSCrunch
{
    public class WatcherService
    {
        private readonly Configurator _configurator;
        private readonly object _syncRoot = new object();
        private readonly Queue<TestRequest> _testRequests;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _isRunning;
        private readonly IOutput _output;

        public WatcherService(Configurator configurator, IOutput output)
        {
            _configurator = configurator;
            _output = output;
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

        private void DumpResults(string workingDirectory)
        {
            var resultFilePath = Path.Combine(workingDirectory, "results.xml");
            if (!File.Exists(resultFilePath))
            {
                return;
            }

            var document = new XmlDocument();

            document.Load(resultFilePath);

            var nodes = document.DocumentElement?.SelectNodes("/testsuites/testsuite");

            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                var fileName = Path.GetFileNameWithoutExtension(node.Attributes["name"].Value);
                var numberOfTests = int.Parse(node.Attributes["tests"].Value);
                var numberOfFailures = int.Parse(node.Attributes["failures"].Value);
                var testcasesThatFailed = node.SelectNodes("testcase[failure]").OfType<XmlNode>().Select(n => n.Attributes["name"].Value).ToList();

                _output.Write(new TestResult
                {
                    TestSuite = fileName,
                    NumberOfTests = numberOfTests,
                    NumberOfFailures = numberOfFailures,
                    FailedTests = testcasesThatFailed
                });
            }
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
            _testRequests.Enqueue(new TestRequest(ApplicationDateTime.UtcNow(), path));
            OnTestRequestQueued();
        }

        private void HandleFileChanged(object sender, FileSystemEventArgs e)
        {
            EnqueueTestRequest(e.FullPath);
        }

        public void Start()
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }

        protected void OnTestRequestQueued()
        {
            TestRequestQueued?.BeginInvoke(this, EventArgs.Empty, null, null);
        }
    }
}