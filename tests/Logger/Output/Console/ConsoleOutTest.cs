using System;
using Chorizo.Logger.Output.Console;
using Moq;
using Xunit;

namespace Chorizo.Tests.Logger.Output.Console
{
    public class ConsoleOutTest
    {
        private const string TestText = "Test";
        private DateTime _testTime = new DateTime(1997, 12, 02, 15, 10, 00, DateTimeKind.Utc);
        private string _testTimeString;

        public ConsoleOutTest()
        {
            _testTimeString = _testTime.ToString("t");
        }


        [Fact]
        public void OutGivenAnErrorOutputsAMessageWithAErrorTag()
        {
            var mockConsole = new Mock<IConsole>();
            var testConsoleOut = new ConsoleOut
            {
                WrappedConsole = mockConsole.Object
            };

            testConsoleOut.Out(TestText, 0, _testTime);

            mockConsole.Verify(console => console.WriteLine($"{_testTimeString} -> [\x1b[31mERROR\x1b[0m] {TestText}"));
        }

        [Fact]
        public void OutGivenAInfoOutputsAMessageWithAInfoTag()
        {
            var mockConsole = new Mock<IConsole>();
            var testConsoleOut = new ConsoleOut
            {
                WrappedConsole = mockConsole.Object
            };

            testConsoleOut.Out(TestText, 1, _testTime);

            mockConsole.Verify(console => console.WriteLine($"{_testTimeString} -> [\x1b[92mINFO\x1b[0m] {TestText}"));
        }

        [Fact]
        public void OutGivenAWarningOutputsAMessageWithAWarningTag()
        {
            var mockConsole = new Mock<IConsole>();
            var testConsoleOut = new ConsoleOut
            {
                WrappedConsole = mockConsole.Object
            };

            testConsoleOut.Out(TestText, 2, _testTime);

            mockConsole.Verify(console => console.WriteLine($"{_testTimeString} -> [\x1b[93mWARNING\x1b[0m] {TestText}"));
        }
    }
}
