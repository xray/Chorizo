using Moq;
using Xunit;

namespace Chorizo.Tests
{
    public class CzoConfigRetrieverTest
    {
        [Fact]
        public void GetConfigOpensCzoConfigJsonAndCreatesAServerConfigFromIt()
        {
            const string testReturnValue = 
                "{\r\n  \"Protocol\": \"telnet\",\r\n  \"HostName\": \"localhost\",\r\n  \"Port\": 23\r\n}";
            var mockReader = new Mock<IFileReader>();
            mockReader.Setup(mr => mr.ReadToEnd()).Returns(testReturnValue);
            var testConfigFetcher = new CzoConfigRetriever(mockReader.Object);

            var result = testConfigFetcher.GetConfig();
            
            mockReader.Verify(mr => mr.ReadToEnd());
            Assert.Equal("telnet", result.Protocol);
            Assert.Equal("localhost", result.HostName);
            Assert.Equal(23, result.Port);
            
        }
    }
}