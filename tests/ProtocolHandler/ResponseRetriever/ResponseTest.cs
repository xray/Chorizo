using Chorizo.ProtocolHandler.HttpHeaders;
using Chorizo.ProtocolHandler.ResponseRetriever;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.ResponseRetriever
{
    public class ResponseTest
    {
        [Fact]
        public void AddHeaderTakesInANameAndValueAndReturnsANewResponseWithTheOriginalResponseDataPlusTheAdditionalHeader()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK");
            var testHeader = new Header("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var resultResponse = testResponse.AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            Assert.Equal("HTTP/1.1", resultResponse.Protocol());
            Assert.Equal(200, resultResponse.StatusCode());
            Assert.Equal("OK", resultResponse.StatusText());
            Assert.True(resultResponse.ContainsHeader("Date"));
            Assert.Equal(testHeader, resultResponse.GetHeader("Date"));
        }

        [Fact]
        public void ToStringUsesTheResponsesPropertiesToCreateAStringThatRepresentsTheResponse()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            const string expectedString = "HTTP/1.1 200 OK\r\n" +
                                          "Date: Tue, 02 Dec 1997 15:10:00 GMT\r\n" +
                                          "\r\n";

            Assert.Equal(expectedString, testResponse.ToString());
        }

        [Fact]
        public void EqualsTakesInASecondaryResponseAndReturnsTrueWhenTheyContainTheSameInformation()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("test", "header");
            var testResponseToCompare = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("test", "header");

            Assert.True(testResponse.Equals(testResponseToCompare));
        }

        [Fact]
        public void EqualsTakesInASecondaryResponseAndReturnsFalseWhenTheyContainDifferentInformation()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("test", "header");
            var testResponseToCompare = new Response("HTTP/1.1", 404, "Not Found")
                .AddHeader("test", "different");

            Assert.False(testResponse.Equals(testResponseToCompare));
        }
    }
}
