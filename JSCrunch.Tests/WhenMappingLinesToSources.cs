using System;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSCrunch.Tests
{
    [TestClass]
    public class WhenMappingLinesToSources
    {
        private const string EncodedContent =
            "TypeError: undefined is not an object (evaluating &apos;result.PortfolioItems&apos;) in file:///C:/LQA/Source/Code/WebADFS/Scripts/Home/Portfolio.Page.js (line 66)&#10;&#9;&#9;at fire (file:///C:/LQA/Source/Code/WebADFS/Scripts/Lib/jquery-2.1.1.js:3073:35)&#10;&#9;&#9;at add (file:///C:/LQA/Source/Code/WebADFS/Scripts/Lib/jquery-2.1.1.js:3119:11)&#10;&#9;&#9;at AddToPortfolio (file:///C:/LQA/Source/Code/WebADFS/Scripts/Home/Portfolio.Page.js:65:26)&#10;&#9;&#9;at file:///c:/lqa/source/code/webadfs/scripts/jasmine/tests/home/WhenAddingClientToPortfolio.Tests.js:20:39&#10;&#9;&#9;at attemptSync (file:///C:/Projects/JSCrunch/packages/Chutzpah.4.2.1/tools/TestFiles/jasmine/v2/jasmine.js:1886:28)";
            
        [TestMethod]
        public void Foo()
        {
            var lines = SourceMapMapper.SourceLinesFromStackTrace(EncodedContent);

            Debug.WriteLine("Splitted:");
            foreach (var line in lines)
            {
                Debug.WriteLine(line);
            }

            lines
                .Should()
                .NotBeNullOrEmpty();
        }

        [TestMethod]
        public void StackTraceContainsMeaningfullMessage()
        {
            var stack = "Expected 0 to be 1.&#10;&#9;&#9;at buildExpectationResult (file:///C:/Projects/JSCrunch/packages/Chutzpah.4.2.1/tools/TestFiles/jasmine/v2/jasmine.js:1547:19)&#10;&#9;&#9;at expectationResultFactory (file:///C:/Projects/JSCrunch/packages/Chutzpah.4.2.1/tools/TestFiles/jasmine/v2/jasmine.js:638:40)&#10;&#9;&#9;at addExpectationResult (file:///C:/Projects/JSCrunch/packages/Chutzpah.4.2.1/tools/TestFiles/jasmine/v2/jasmine.js:330:58)&#10;&#9;&#9;at addExpectationResult (file:///C:/Projects/JSCrunch/packages/Chutzpah.4.2.1/tools/TestFiles/jasmine/v2/jasmine.js:588:41)&#10;&#9;&#9;at file:///C:/Projects/JSCrunch/packages/Chutzpah.4.2.1/tools/TestFiles/jasmine/v2/jasmine.js:1501:32";

            var lines = SourceMapMapper.SourceLinesFromStackTrace(stack);


        }
    }
}