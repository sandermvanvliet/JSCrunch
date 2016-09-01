//------------------------------------------------------------------------------
// <copyright file="EventLogToolWindow.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using JSCrunch.Core;
using Microsoft.Practices.Unity;

namespace JSCrunch.VisualStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("339e2369-e379-4057-96db-83d3a6e2b2f5")]
    public sealed class ProcessingQueueToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingQueueToolWindow"/> class.
        /// </summary>
        public ProcessingQueueToolWindow() : base(null)
        {
            this.Caption = "JSCrunch Processing Queue";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.

            Content = new EventLogControl();
        }

        public override void OnToolWindowCreated()
        {
            var unityContainer = ((IServiceProvider) Package).GetService(typeof(IUnityContainer)) as IUnityContainer;
            var eventQueue = unityContainer.Resolve<EventQueue>();
            ((EventLogControl) Content).EventQueue = eventQueue;
        }
    }
}
