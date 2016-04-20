using System;
using System.Configuration;

namespace JSCrunch
{
    public class Configurator
    {
        public Configurator()
        {
            Refresh();
        }

        public void Refresh()
        {
            PathToWatch = ConfigurationManager.AppSettings["JSCrunch.PathToWatch"];
            TestPattern = ConfigurationManager.AppSettings["JSCrunch.TestPattern"];
            TestRunnerExecutable = ConfigurationManager.AppSettings["JSCrunch.TestRunnerExecutable"];
            TestRunnerParameters = ConfigurationManager.AppSettings["JSCrunch.TestRunnerParameters"];

            OnUpdateSettingsAvailable();
        }

        public string TestPattern { get; private set; }

        public string PathToWatch { get; private set; }
        public string TestRunnerParameters { get; private set; }
        public string TestRunnerExecutable { get; private set; }

        public event EventHandler<EventArgs> UpdatedSettingsAvailable;

        private void OnUpdateSettingsAvailable()
        {
            UpdatedSettingsAvailable?.Invoke(this, EventArgs.Empty);
        }
    }
}