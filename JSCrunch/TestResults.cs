﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace JSCrunch
{
    public class TestResults
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
                var testcasesThatFailed =
                    node.SelectNodes("testcase[failure]")
                        .OfType<XmlNode>()
                        .Select(n => new TestCaseResult { Name = n.Attributes["name"].Value, Output = n.SelectSingleNode("failure").Attributes["message"].Value })
                        .ToList();

                testResults.Add(new TestResult
                {
                    TestSuite = fileName,
                    NumberOfTests = numberOfTests,
                    NumberOfFailures = numberOfFailures,
                    FailedTests = testcasesThatFailed
                });
            }

            return testResults;
        }

    public class TestCaseResult
    {
        public string Name { get; set; }
        public string Output { get; set; }
    }
}
}