using JSCrunch.Core;

namespace JSCrunch.Tests
{
    public class DummyConfigurator : Configurator
    {
        protected override string GetAppSetting(string name)
        {
            switch (name)
            {
                case "JSCrunch.IncludeSubdirectories":
                    return bool.TrueString;
                case "JSCrunch.TestRunnerExecutable":
                    return "cmd.exe";
                case "JSCrunch.TestRunnerParameters":
                    return "/C \"hello world!\"";
                default:
                    return "";
            }
        }
    }
}