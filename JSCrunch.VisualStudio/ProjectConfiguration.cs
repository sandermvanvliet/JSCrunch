using System.Xml;

namespace JSCrunch.VisualStudio
{
    public class ProjectConfiguration
    {
        public string TestPattern { get; set; }
        public string TestRoot { get; set; }

        public static ProjectConfiguration FromContents(string contents)
        {
            var document = new XmlDocument();

            document.LoadXml(contents);

            var testsElement = document.SelectSingleNode("/jscrunch/tests");
            
            var projectConfiguration = new ProjectConfiguration
            {
                TestPattern = testsElement.Attributes["pattern"].Value,
                TestRoot = testsElement.Attributes["root"].Value
            };

            return projectConfiguration;
        }
    }
}