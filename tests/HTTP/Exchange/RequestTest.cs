using Chorizo.HTTP.Exchange;
using Xunit;

namespace Chorizo.Tests.HTTP.Exchange
{
    public class RequestTest
    {
        [Fact]
        public void ToStringUsesTheRequestsPropertiesToCreateAStringThatRepresentsAHttpRequest()
        {
            var testHeaders = new Headers()
                .AddHeader("test", "header");
            var testRequest = new Request("GET", "/", "HTTP/1.1", testHeaders);

            const string expectedString = "GET / HTTP/1.1\r\n" +
                                          "test: header\r\n" +
                                          "\r\n";

            Assert.Equal(expectedString, testRequest.ToString());
        }

        [Fact]
        public void EqualsTakesInASecondaryRequestAndReturnsTrueWhenTheyContainTheSameInformation()
        {
            var testHeaders = new Headers()
                .AddHeader("test", "header");
            var testRequest = new Request("GET", "/", "HTTP/1.1", testHeaders);
            var testRequestToCompare = new Request("GET", "/", "HTTP/1.1", testHeaders);

            Assert.True(testRequest.Equals(testRequestToCompare));
        }

        [Fact]
        public void EqualsTakesInASecondaryRequestAndReturnsFalseWhenTheyContainDifferentInformation()
        {
            var testHeaders = new Headers()
                .AddHeader("test", "header");
            var testRequest = new Request("GET", "/", "HTTP/1.1", testHeaders);
            var testRequestToCompare = new Request("GET", "/hello_world", "HTTP/1.1", testHeaders);

            Assert.False(testRequest.Equals(testRequestToCompare));
        }
    }
}
