using System;
using System.IO;
using JSCrunch.Core;
using JSCrunch.Core.Events;

namespace JSCrunch
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // WatcherService is only ever instantiated through Unity
    public class WatcherService
    {
        private readonly AppConfigConfigurator _configurator;
        private readonly EventQueue _eventQueue;
        private FileSystemWatcher _fileSystemWatcher;

        public WatcherService(AppConfigConfigurator configurator, EventQueue eventQueue)
        {
            _configurator = configurator;
            _eventQueue = eventQueue;
            _configurator.UpdatedSettingsAvailable += HandleUpdatedSettingsAvailable;

            InitializeFileWatcher(configurator);
        }
        
        private void HandleUpdatedSettingsAvailable(object sender, EventArgs e)
        {
            InitializeFileWatcher(_configurator);
        }

        private void InitializeFileWatcher(AppConfigConfigurator configurator)
        {
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
                _fileSystemWatcher.Dispose();
                _fileSystemWatcher = null;
            }

            _fileSystemWatcher = new FileSystemWatcher(configurator.PathToWatch, configurator.TestPattern)
            {
                IncludeSubdirectories = configurator.IncludeSubdirectories
            };

            _fileSystemWatcher.Changed += HandleFileChanged;
            _fileSystemWatcher.Created += HandleFileChanged;
            _fileSystemWatcher.Renamed += HandleFileRenamed;
        }

        private void HandleFileRenamed(object sender, RenamedEventArgs e)
        {
            EnqueueTestRequest(e.FullPath);
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

        private void EnqueueTestRequest(string path)
        {
            _eventQueue.Enqueue(new FileChangedEvent(path));
        }
    }
}