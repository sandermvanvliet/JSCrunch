﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JSCrunch.Core;
using JSCrunch.Core.Events;
using JSCrunch.VisualStudio.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace JSCrunch.VisualStudio.Tests
{
    [TestClass]
    public class WhenAProjectIsLoaded
    {
        [TestMethod]
        public void ThenADiscoverTestsEventIsQueued()
        {
            var eventQueue = new EventQueue();
            var eventHandler = new VisualStudioEventHandler(new List<ProcessingItem>(), eventQueue, Substitute.For<IServiceProvider>());

            eventHandler.OnAfterLoadProject(null, null);

            eventQueue
                .OfType<ProjectLoadedEvent>()
                .Should()
                .HaveCount(1);
        }
    }
}