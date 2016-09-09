using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace JSCrunch.Core
{
    public static class TestResultBuilder
    {
        public static List<TestResult> From(string workingDirectory)
        {
            var testResults = new List<TestResult>();

            var resultFilePath = Path.Combine(workingDirectory, "results.xml");
            if (!File.Exists(resultFilePath))
            {
                return testResults;
            }

            var document = new XmlDocument();

            document.Load(resultFilePath);

            var nodes = document.DocumentElement?.SelectNodes("/testsuites/testsuite");

            if (nodes == null)
            {
                return testResults;
            }

            testResults = new List<TestResult>();

            foreach (XmlNode node in nodes)
            {
                var fileName = Path.GetFileNameWithoutExtension(node.Attributes["name"].Value);
                var numberOfTests = int.Parse(node.Attributes["tests"].Value);
                var numberOfFailures = int.Parse(node.Attributes["failures"].Value);

                var testCases = node.SelectNodes("testcase")
                    .OfType<XmlNode>()
                    .Select(AsTestCaseResult)
                    .ToList();

                testResults.Add(new TestResult
                {
                    TestSuite = fileName,
                    NumberOfTests = numberOfTests,
                    NumberOfFailures = numberOfFailures,
                    Tests = testCases
                });
            }

            return testResults;
        }

        private static TestCaseResult AsTestCaseResult(XmlNode node)
        {
            var result = new TestCaseResult { Name = node.Attributes["name"].Value, Success = true };

            var failure = node.SelectSingleNode("failure");
            
            if (failure != null)
            {
                result.Success = false;

                result.Output = MapOutput(node);
            }

            return result;
        }

        private static string MapOutput(XmlNode n)
        {
            var message = n.SelectSingleNode("failure").Attributes["message"].Value;

            return SourceMapMapper.SourceLinesFromStackTrace(message);
        }
    }
}