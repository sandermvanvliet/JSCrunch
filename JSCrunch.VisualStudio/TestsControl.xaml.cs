//------------------------------------------------------------------------------
// <copyright file="TestsControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;
using Microsoft.VisualStudio.Shell.Interop;

namespace JSCrunch.VisualStudio
{
    /// <summary>
    ///     Interaction logic for TestsControl.
    /// </summary>
    public partial class TestsControl : UserControl, ISubscribable<UpdateMetadataEvent>
    {
        private EventQueue _eventQueue;
        private bool _bufferEvents = true;
        private readonly Queue<UpdateMetadataEvent> _eventBuffer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestsControl" /> class.
        /// </summary>
        public TestsControl()
        {
            InitializeComponent();
            TreeCollection = new ObservableCollection<Node>();
            DataContext = this;
            _eventBuffer = new Queue<UpdateMetadataEvent>();
        }

        public EventQueue EventQueue
        {
            get { return _eventQueue; }
            set
            {
                if (_eventQueue == null)
                {
                    _eventQueue = value;
                    _eventQueue.Subscribe(this);
                }
            }
        }

        public ObservableCollection<Node> TreeCollection { get; }

        public Type ForEventType => typeof(UpdateMetadataEvent);

        public void Publish(UpdateMetadataEvent eventInstance)
        {
            if (_bufferEvents)
            {
                _eventBuffer.Enqueue(eventInstance);
                return;
            }

            if (eventInstance is SolutionOpenedEvent)
            {
                var solutionOpenedEvent = (SolutionOpenedEvent) eventInstance;
                object pvar;
                solutionOpenedEvent.Solution.GetProperty((int) __VSPROPID.VSPROPID_SolutionBaseName, out pvar);
                var solutionName = (string) pvar;

                if (TreeCollection.All(n => n.Name != solutionName))
                {
                    TreeCollection.Clear();
                    TreeCollection.Add(new Node
                    {
                        Name = solutionName
                    });
                }
            }

            if (eventInstance is ProjectLoadedEvent)
            {
                var projectLoadedEvent = (ProjectLoadedEvent) eventInstance;

                var solution = TreeCollection.FirstOrDefault();
                if (solution == null)
                {
                    solution = new Node {Name = "Solution!"};
                    TreeCollection.Add(solution);
                }

                var projectExists = solution.Children.Any(c => c.Name == projectLoadedEvent.Project.GetProjectName());
                if (!projectExists)
                {
                    solution
                        .Children
                        .Add(new Node {Name = projectLoadedEvent.Project.GetProjectName()});
                }
            }

            if (eventInstance is TestsFoundEvent)
            {
                var testsFoundEvent = (TestsFoundEvent) eventInstance;
                var solution = TreeCollection.FirstOrDefault();
                if (solution == null)
                {
                    solution = new Node {Name = "Solution!"};
                    TreeCollection.Add(solution);
                }

                var project = solution.Children.SingleOrDefault(c => c.Name == testsFoundEvent.ProjectName);
                if (project != null)
                {
                    foreach (var test in testsFoundEvent.Tests)
                    {
                        var exists = project.Children.Any(c => c.Name == test.Name);
                        if (!exists)
                        {
                            project
                                .Children
                                .Add(new Node {Name = test.Name});
                        }
                    }
                }
            }
        }

        private void HandleOnLoaded(object sender, RoutedEventArgs e)
        {
            // Replay buffered events
            _bufferEvents = false;
            while (_eventBuffer.Any())
            {
                Publish(_eventBuffer.Dequeue());
            }
        }
    }

    public class Node
    {
        public Node()
        {
            Children = new ObservableCollection<Node>();
        }

        public string Name { get; set; }
        public ObservableCollection<Node> Children { get; private set; }
    }
}