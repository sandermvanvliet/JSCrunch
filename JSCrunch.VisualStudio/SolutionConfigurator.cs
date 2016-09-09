using JSCrunch.Core;

namespace JSCrunch.VisualStudio
{
    public class SolutionConfigurator : IConfigurator
    {
        public string TestPattern { get; set; }
        public string PathToWatch { get; set; }
        public string TestRunnerParameters { get; set; }
        public string TestRunnerExecutable { get; set; }
        public bool IncludeSubdirectories { get; set; }
    }
}