using System;
using System.IO;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;

namespace JSCrunch.VisualStudio.Listeners
{
    public class DiscoverTestsListener : ISubscribable<DiscoverTestsEvent>
    {
        private readonly IFileSystem _fileSystem;

        public DiscoverTestsListener(EventQueue eventQueue, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ProjectConfiguration ProjectConfiguration { get; set; }

        public Type ForEventType => typeof(DiscoverTestsEvent);

        public void Publish(DiscoverTestsEvent eventInstance)
        {
            if (eventInstance
                .Project
                .HasFile("jscrunch.config"))
            {
                var configPath = Path.Combine(eventInstance.Project.GetProjectDir(), "jscrunch.config");

                var contents = _fileSystem.GetContentsOf(configPath);

                ProjectConfiguration = ProjectConfiguration.FromContents(contents);
            }
        }
    }
}