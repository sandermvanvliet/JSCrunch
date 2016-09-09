using System;
using System.Configuration;

namespace JSCrunch.Core
{
    public interface IConfigurator
    {
        string TestPattern { get; }
        string PathToWatch { get; }
        string TestRunnerParameters { get; }
        string TestRunnerExecutable { get; set; }
        bool IncludeSubdirectories { get; }
    }

    public class AppConfigConfigurator : IConfigurator
    {
        public AppConfigConfigurator()
        {
            Refresh();
        }

        public void Refresh()
        {
            PathToWatch = GetAppSetting("JSCrunch.PathToWatch");
            TestPattern = GetAppSetting("JSCrunch.TestPattern");
            IncludeSubdirectories = bool.Parse(GetAppSetting("JSCrunch.IncludeSubdirectories"));
            TestRunnerExecutable = GetAppSetting("JSCrunch.TestRunnerExecutable");
            TestRunnerParameters = GetAppSetting("JSCrunch.TestRunnerParameters");

            OnUpdateSettingsAvailable();
        }

        protected virtual string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        public string TestPattern { get; private set; }

        public string PathToWatch { get; private set; }
        public string TestRunnerParameters { get; private set; }
        public string TestRunnerExecutable { get; set; }
        public bool IncludeSubdirectories { get; private set; }

        public event EventHandler<EventArgs> UpdatedSettingsAvailable;

        private void OnUpdateSettingsAvailable()
        {
            UpdatedSettingsAvailable?.Invoke(this, EventArgs.Empty);
        }
    }
}