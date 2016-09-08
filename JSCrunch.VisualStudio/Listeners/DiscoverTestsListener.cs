using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet;

namespace JSCrunch.VisualStudio.Listeners
{
    public class DiscoverTestsListener : ISubscribable<DiscoverTestsEvent>
    {
        private readonly EventQueue _eventQueue;
        private readonly IFileSystem _fileSystem;

        public DiscoverTestsListener(EventQueue eventQueue, IFileSystem fileSystem)
        {
            _eventQueue = eventQueue;
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

                var directories = ProjectConfiguration.TestRoot.Split(Path.DirectorySeparatorChar);

                var project = ((IVsHierarchy)eventInstance.Project).GetEnvDteProject();

                var first = project.ProjectItems.OfType<ProjectItem>().SingleOrDefault(p => p.Name == directories[0]);

                if (first != null)
                {
                    var testRoot = first;
                    if (directories.Length > 1)
                    {
                        testRoot = DrillDown(first, directories, 1);
                    }

                    if (testRoot != null)
                    {
                        var tests = EnumerateTests(testRoot, ProjectConfiguration.TestPattern);
                         _eventQueue.Enqueue(new TestsFoundEvent(tests, eventInstance.Project.GetProjectName()));
                    }
                }
            }
        }

        private IEnumerable<ProjectItem> EnumerateTests(ProjectItem testRoot, string testPattern)
        {
            var items = testRoot
                .ProjectItems
                .OfType<ProjectItem>()
                .ToList();

            foreach (var item in items)
            {
                if (MatchesPattern(item.Name, testPattern))
                {
                    yield return item;
                }
                else if (item.ProjectItems.Count > 0)
                {
                    foreach (var subItem in EnumerateTests(item, testPattern))
                    {
                        yield return subItem;
                    }
                }
            }
        }

        private bool MatchesPattern(string name, string testPattern)
        {
            return Regex.IsMatch(name, testPattern);
        }

        private static ProjectItem DrillDown(ProjectItem projectItem, string[] directories, int pointer)
        {
            var next = projectItem
                .ProjectItems
                .OfType<ProjectItem>()
                .SingleOrDefault(p => p.Name == directories[pointer]);

            if (next != null)
            {
                if (pointer == directories.Length - 1)
                {
                    return next;
                }
                if (next.ProjectItems.Count > 0)
                {
                    return DrillDown(next, directories, pointer + 1);
                }
            }
            
            return null;
        }
    }
}