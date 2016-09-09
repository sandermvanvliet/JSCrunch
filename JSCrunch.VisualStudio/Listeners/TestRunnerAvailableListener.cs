using System;
using JSCrunch.Core;
using JSCrunch.VisualStudio.Events;

namespace JSCrunch.VisualStudio.Listeners
{
    public class TestRunnerAvailableListener : ISubscribable<TestRunnerAvailableEvent>
    {
        private readonly IConfigurator _configurator;

        public TestRunnerAvailableListener(IConfigurator configurator)
        {
            _configurator = configurator;
        }

        public Type ForEventType => typeof(TestRunnerAvailableEvent);

        public void Publish(TestRunnerAvailableEvent eventInstance)
        {
            _configurator.TestRunnerExecutable = eventInstance.TestRunnerPath;
        }
    }
}