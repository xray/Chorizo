using Chorizo.HTTP.Exchange;
using Xunit;

namespace Chorizo.Tests.HTTP.Exchange
{
    public class ResponseTest
    {
        [Fact]
        public void AddHeaderTakesInANameAndValueAndReturnsANewResponseWithTheOriginalResponseDataPlusTheAdditionalHeader()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK", "test body");
            var testHeader = new Header("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var resultResponse = testResponse.AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            Assert.Equal("HTTP/1.1", resultResponse.Protocol);
            Assert.Equal(200, resultResponse.StatusCode);
            Assert.Equal("OK", resultResponse.StatusText);
            Assert.True(resultResponse.ContainsHeader("Date"));
            Assert.Equal(testHeader, resultResponse.GetHeader("Date"));
            Assert.Equal("test body", resultResponse.Body);
        }

        [Fact]
        public void ToStringUsesTheResponsesPropertiesToCreateAStringThatRepresentsTheResponseWithoutBody()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            const string expectedString = "HTTP/1.1 200 OK\r\n" +
                                          "Date: Tue, 02 Dec 1997 15:10:00 GMT\r\n" +
                                          "\r\n";

            Assert.Equal(expectedString, testResponse.ToString());
        }

        [Theory]
        [InlineData("test body", 9)]
        [InlineData("<html><h1>Chorizo</h1></html>", 29)]
        public void ToStringUsesTheResponsesPropertiesToCreateAStringThatRepresentsTheResponseWithBody(string body, int contentLength)
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK", $"{body}")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var expectedString = "HTTP/1.1 200 OK\r\n" +
                                 "Date: Tue, 02 Dec 1997 15:10:00 GMT\r\n" +
                                 $"Content-Length: {contentLength}\r\n" +
                                 "\r\n" +
                                 $"{body}";

            Assert.Equal(expectedString, testResponse.ToString());
        }

        [Fact]
        public void EqualsTakesInASecondaryResponseAndReturnsTrueWhenTheyContainTheSameInformation()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK", "test body")
                .AddHeader("test", "header");
            var testResponseToCompare = new Response("HTTP/1.1", 200, "OK", "test body")
                .AddHeader("test", "header");

            Assert.True(testResponse.Equals(testResponseToCompare));
        }

        [Fact]
        public void EqualsTakesInASecondaryResponseAndReturnsFalseWhenTheyContainDifferentInformation()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("test", "header");
            var testResponseToCompare = new Response("HTTP/1.1", 200, "OK", "test body")
                .AddHeader("test", "header");

            Assert.False(testResponse.Equals(testResponseToCompare));
        }
    }
}
