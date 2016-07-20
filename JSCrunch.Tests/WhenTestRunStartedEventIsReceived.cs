using FluentAssertions;
using JSCrunch.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.Tests
{
    [TestClass]
    public class WhenTestRunStartedEventIsReceived
    {
        [TestMethod]
        public void ThenAMessageIsWrittenToTheOutput()
        {
            var output = new OutputDouble();
            var listener = new TestRunStartedListener(output);
            listener.Publish(new TestRunStartedEvent(@"c:\temp\doesntexist.js"));

            output
                .Messages
                .Should()
                .Contain("Test run started for 'c:\\temp\\doesntexist.js'");
        }
    }
}