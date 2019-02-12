using System;
using Chorizo.Logger;
using Chorizo.Logger.Configuration;
using Chorizo.Logger.Output;
using Moq;
using Xunit;

namespace Chorizo.Tests.Logger
{
    public class MiniLoggerTest
    {
        private readonly DateTime _testTime = new DateTime(1997, 12, 02, 03, 32, 00, DateTimeKind.Utc);
        private readonly Mock<IDateTimeProvider> _mockDateTime;

        public MiniLoggerTest()
        {
            _mockDateTime = new Mock<IDateTimeProvider>();
            _mockDateTime.Setup(dt => dt.Now()).Returns(_testTime);
        }
        
        [Fact]
        public void WhenConfiguredWithDevLevelDisplayAllLogs()
        {
            var mockUIOut = new Mock<ILoggerOut>();
            var miniLogger = new MiniLogger(
                new LogConfig("dev"),
                new []{mockUIOut.Object},
                _mockDateTime.Object
            );
            
            const string testMsgOne = "Message 1";
            const string testMsgTwo = "Message 2";
            const string testMsgThree = "Message 3";
            
            miniLogger.Error(testMsgOne);
            miniLogger.Info(testMsgTwo);
            miniLogger.Warning(testMsgThree);
            
            mockUIOut.Verify(ui => ui.Out(testMsgOne, 0, _testTime));
            mockUIOut.Verify(ui => ui.Out(testMsgTwo, 1, _testTime));
            mockUIOut.Verify(ui => ui.Out(testMsgThree, 2, _testTime));
            mockUIOut.Verify(ui => ui.Out(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Exactly(3));
        }

        [Fact]
        public void WhenConfiguredWithProdLevelDisplayOnlyErrorsAndInfo()
        {
            var mockUIOut = new Mock<ILoggerOut>();
            var miniLogger = new MiniLogger(
                new LogConfig("prod"),
                new []{mockUIOut.Object},
                _mockDateTime.Object
            );
            
            const string testMsgOne = "Message 1";
            const string testMsgTwo = "Message 2";
            const string testMsgThree = "Message 3";
            
            miniLogger.Error(testMsgOne);
            miniLogger.Info(testMsgTwo);
            miniLogger.Warning(testMsgThree);
            
            mockUIOut.Verify(ui => ui.Out(testMsgOne, 0, _testTime));
            mockUIOut.Verify(ui => ui.Out(testMsgTwo, 1, _testTime));
            mockUIOut.Verify(ui => ui.Out(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Exactly(2));
        }
        
        [Fact]
        public void WhenConfiguredWithTestLevelDisplayOnlyErrors()
        {
            var mockUIOut = new Mock<ILoggerOut>();
            var miniLogger = new MiniLogger(
                new LogConfig("test"),
                new []{ mockUIOut.Object },
                _mockDateTime.Object
            );
            
            const string testMsgOne = "Message 1";
            const string testMsgTwo = "Message 2";
            const string testMsgThree = "Message 3";
            
            miniLogger.Error(testMsgOne);
            miniLogger.Info(testMsgTwo);
            miniLogger.Warning(testMsgThree);
            
            mockUIOut.Verify(ui => ui.Out(testMsgOne, 0, _testTime));
            mockUIOut.Verify(ui => ui.Out(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
