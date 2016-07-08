//------------------------------------------------------------------------------
// <copyright file="ProcessingQueueControl.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
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
    public partial class ProcessingQueueControl : UserControl, IVsRunningDocTableEvents
    {
        private uint _rdtCookie;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessingQueueControl" /> class.
        /// </summary>
        public ProcessingQueueControl()
        {
            InitializeComponent();
        }

        private void ProcessingQueueControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var serviceProvider = Package.GetGlobalService(typeof(IOleServiceProvider)) as IOleServiceProvider;
            if (serviceProvider == null)
            {
                return;
            }

            var rdt = new RunningDocumentTable(new ServiceProvider(serviceProvider));
            if (rdt == null)
            {
                return;
            }

            _rdtCookie = rdt.Advise(this);

        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
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
    }
}