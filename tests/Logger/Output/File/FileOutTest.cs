using System;
using Chorizo.Logger.Output.File;
using Moq;
using Xunit;

namespace Chorizo.Tests.Logger.Output.File
{
    public class FileOutTest
    {
        private readonly DateTime _testTime = new DateTime(1997, 12, 02, 03, 32, 00, DateTimeKind.Utc);
        private const string  _testFileNameTimeString = "1997-12-02-033200";
        private const string _testInitializationTimeString = "Dec 2, 1997 @ 03:32 AM (Z)";
        private const string _testLogTimeString = "02/Dec/1997:03:32:00 +00:00";
        private const string TestFileName = "TestLog";
        private const string TestDirectory = @"/test/123/";
        private const string TestText = "Test";

        private string _testFilePath;

        private Mock<IFileWriter> _mockDotNetFile;

        public FileOutTest()
        {
            _mockDotNetFile = new Mock<IFileWriter>();
            _testFilePath = $"{TestDirectory}{_testFileNameTimeString}_{TestFileName}.md";
        }

        [Fact]
        public void GivenAnyMessageExpectAFileToBeCreatedAndHaveInitialLinesWritten()
        {
            var testFileOut = new FileOut(
                _testTime, 
                TestDirectory, 
                TestFileName, 
                _mockDotNetFile.Object
            );
            
            var expectedInitializationList = new[]
            {
                $"# {TestFileName}  ",
                $"#### Initialized On {_testInitializationTimeString}  ",
                "---",
                "---",
                ""
            };
            
            testFileOut.Out(TestText, 2, _testTime);
            
            _mockDotNetFile.Verify(file => file.CreateDirectory(TestDirectory));
            _mockDotNetFile.Verify(file => file.WriteAllLines(_testFilePath, expectedInitializationList));
        }

        [Fact]
        public void GivenAnErrorMessageExpectAnInitializedFileToHaveErrorMessageLinesAppended()
        {
            var testFileOut = new FileOut(
                _testTime, 
                TestDirectory, 
                TestFileName, 
                _mockDotNetFile.Object
            );
            
            var expectedLogList = new[]
            {
                "## Error  ",
                $"**Time**: {_testLogTimeString}",
                $"> {TestText}",
                "---"
            };
            
            testFileOut.Out(TestText, 0, _testTime);
            
            _mockDotNetFile.Verify(file => file.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
            _mockDotNetFile.Verify(file => file.AppendAllLines(_testFilePath, expectedLogList));
            _mockDotNetFile.Verify(file => file.AppendAllLines(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
        }
        
        [Fact]
        public void GivenAInfoMessageExpectAnInitializedFileToHaveInfoMessageLinesAppended()
        {
            var testFileOut = new FileOut(
                _testTime, 
                TestDirectory, 
                TestFileName, 
                _mockDotNetFile.Object
            );
            
            var expectedLogList = new[]
            {
                "## Info  ",
                $"**Time**: {_testLogTimeString}",
                $"> {TestText}",
                "---"
            };
            
            testFileOut.Out(TestText, 1, _testTime);
            
            _mockDotNetFile.Verify(file => file.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
            _mockDotNetFile.Verify(file => file.AppendAllLines(_testFilePath, expectedLogList));
            _mockDotNetFile.Verify(file => file.AppendAllLines(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
        }
        
        [Fact]
        public void GivenAWarningMessageExpectAnInitializedFileToHaveWarningMessageLinesAppended()
        {
            var testFileOut = new FileOut(
                _testTime, 
                TestDirectory, 
                TestFileName, 
                _mockDotNetFile.Object
            );
            
            var expectedLogList = new[]
            {
                "## Warning  ",
                $"**Time**: {_testLogTimeString}",
                $"> {TestText}",
                "---"
            };
            
            testFileOut.Out(TestText, 2, _testTime);
            
            _mockDotNetFile.Verify(file => file.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
            _mockDotNetFile.Verify(file => file.AppendAllLines(_testFilePath, expectedLogList));
            _mockDotNetFile.Verify(file => file.AppendAllLines(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once);
        }
    }
}