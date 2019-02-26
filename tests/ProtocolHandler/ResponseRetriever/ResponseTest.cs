using Chorizo.ProtocolHandler.ResponseRetriever;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.ResponseRetriever
{
    public class ResponseTest
    {
        [Fact]
        public void AddHeaderTakesInAHeaderAndReturnsANewResponseWithTheOriginalResponseDataPlusTheAdditionalHeader()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK");
            var testHeaderArray = new[] { new Header("Date", "Tue, 02 Dec 1997 15:10:00 GMT") };

            var resultResponse = testResponse.AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            Assert.Equal("HTTP/1.1", resultResponse.Protocol());
            Assert.Equal(200, resultResponse.StatusCode());
            Assert.Equal("OK", resultResponse.StatusText());
            Assert.Equal(1, resultResponse.Headers().Length);
            Assert.Equal(testHeaderArray, resultResponse.Headers());
        }
    }
}
