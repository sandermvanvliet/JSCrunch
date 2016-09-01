//------------------------------------------------------------------------------
// <copyright file="ProcessingQueueControl.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MsVsShell = Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace JSCrunch.VisualStudio
{
    /// <summary>
    ///     Interaction logic for ProcessingQueueControl.
    /// </summary>
    public partial class ProcessingQueueControl : UserControl, IVsRunningDocTableEvents, ISubscribable<Event>
    {
        private EventQueue _eventQueue;
        private MsVsShell.RunningDocumentTable _rdt;
        private uint _rdtCookie;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessingQueueControl" /> class.
        /// </summary>
        public ProcessingQueueControl()
        {
            Collection = new ObservableCollection<Event>();
            DataContext = this;
            InitializeComponent();
        }

        public ObservableCollection<Event> Collection { get; }

        public Type ForEventType => typeof(Event);

        public EventQueue EventQueue
        {
            get { return _eventQueue; }
            set
            {
                _eventQueue = value;
                _eventQueue.Subscribe(this);
            }
        }

        public void Publish(Event eventInstance)
        {
            Collection.Add(eventInstance);
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining,
            uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining,
            uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            var documentInfo = _rdt.GetDocumentInfo(docCookie);

            string fileName;

            if (documentInfo.Hierarchy.GetCanonicalName(documentInfo.ItemId, out fileName) == VSConstants.S_OK)
            {
                AddItemToQueue(fileName);
            }

            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        private void ProcessingQueueControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var oleServiceProvider =
                MsVsShell.Package.GetGlobalService(typeof(IOleServiceProvider)) as IOleServiceProvider;
            if (oleServiceProvider == null)
            {
                return;
            }

            _rdt = new MsVsShell.RunningDocumentTable(new MsVsShell.ServiceProvider(oleServiceProvider));

            _rdtCookie = _rdt.Advise(this);
        }

        private void AddItemToQueue(string fileName)
        {
            //var processingItem = new ProcessingItem
            //{
            //    Action = "FileSave",
            //    Status = "Queued",
            //    Timestamp = DateTime.UtcNow,
            //    FileName = fileName
            //};

            //ThreadPool.QueueUserWorkItem(x =>
            //{
            //    Dispatcher.Invoke(() => Collection.Add(processingItem));
            //});
        }
    }
}