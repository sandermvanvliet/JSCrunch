//------------------------------------------------------------------------------
// <copyright file="EventLogControl.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using MsVsShell = Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace JSCrunch.VisualStudio
{
    /// <summary>
    ///     Interaction logic for EventLogControl.
    /// </summary>
    public partial class EventLogControl : UserControl, ISubscribable<Event>
    {
        private EventQueue _eventQueue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventLogControl" /> class.
        /// </summary>
        public EventLogControl()
        {
            Collection = new ObservableCollection<Event>();
            DataContext = this;
            InitializeComponent();
        }

        public ObservableCollection<Event> Collection { get; }

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

        public void Publish(Event eventInstance)
        {
            Collection.Add(eventInstance);
        }

        private void Handle_OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}