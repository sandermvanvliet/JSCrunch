//------------------------------------------------------------------------------
// <copyright file="TestsControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using JSCrunch.VisualStudio.Events;
using JSCrunch.VisualStudio.Metadata;

namespace JSCrunch.VisualStudio
{
    /// <summary>
    ///     Interaction logic for TestsControl.
    /// </summary>
    [SubscribableOptions(LoadDeferred=true)]
    public partial class TestsControl : UserControl, ISubscribable<MetadataChangedEvent>
    {
        private EventQueue _eventQueue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestsControl" /> class.
        /// </summary>
        public TestsControl()
        {
            InitializeComponent();
            TreeCollection = new ObservableCollection<MetadataModel>();
            DataContext = this;
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

        public ObservableCollection<MetadataModel> TreeCollection { get; set; }

        public Type ForEventType => typeof(MetadataChangedEvent);

        public void Publish(MetadataChangedEvent eventInstance)
        {
            if (!IsLoaded)
            {
                return;
            }

            TreeCollection.Clear();
            TreeCollection.Add(eventInstance.Model);
        }

        private void HandleOnLoaded(object sender, RoutedEventArgs e)
        {
            _eventQueue.Enqueue(new MetadataRequestedEvent());
        }
    }

    public class MetadataRequestedEvent : Event
    {
    }
}