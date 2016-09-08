//------------------------------------------------------------------------------
// <copyright file="TestsToolWindowPane.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using JSCrunch.Core;
using JSCrunch.VisualStudio.Metadata;
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
    [Guid("fd07f1aa-97d8-4461-9be3-236b116b22a3")]
    public class TestsToolWindowPane : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestsToolWindowPane"/> class.
        /// </summary>
        public TestsToolWindowPane() : base(null)
        {
            this.Caption = "Tests";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new TestsControl();
        }

        public override void OnToolWindowCreated()
        {
            var unityContainer = ((IServiceProvider)Package).GetService(typeof(IUnityContainer)) as IUnityContainer;
            var eventQueue = unityContainer.Resolve<EventQueue>();
            var testsControl = ((TestsControl)Content);
            testsControl.EventQueue = eventQueue;
        }
    }
}
