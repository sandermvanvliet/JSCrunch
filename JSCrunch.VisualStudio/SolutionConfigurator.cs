using JSCrunch.Core;

namespace JSCrunch.VisualStudio
{
    public class SolutionConfigurator : IConfigurator
    {
        public SolutionConfigurator()
        {
            TestRunnerParameters =
                "\"{0}\" /testMode TypeScript /UseSourceMaps /debug /trace /junit results.xml";
        }
        public string TestPattern { get; set; }
        public string PathToWatch { get; set; }
        public string TestRunnerParameters { get; set; }
        public string TestRunnerExecutable { get; set; }
        public bool IncludeSubdirectories { get; set; }
    }
}