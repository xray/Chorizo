using System.Collections.Generic;
using System.Text;
using Chorizo.ProtocolHandler.DataParser;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.DataParser
{
    public class RequestParserTest
    {
        [Fact]
        public void ParseTakesInBytesOfARequestWithNoHeadersAndReturnsARequest()
        {
            const string testGetRequestString = "GET / HTTP/1.1\r\n" +
                                                "\r\n";
            var testGetRequestBytes = Encoding.UTF8.GetBytes(testGetRequestString);

            var testRequest = new Request(
                "GET",
                "/",
                "HTTP/1.1"
            );

            var testDataParser = new RequestParser();
            var result = testDataParser.Parse(testGetRequestBytes);

            Assert.True(result.Equals(testRequest));
        }

        [Fact]
        public void ParseTakesInBytesOfARequestWithHeadersAndReturnsARequest()
        {
            const string testGetRequestString = "GET / HTTP/1.1\r\n" +
                                                "foo: bar\r\n" +
                                                "\r\n";
            var testGetRequestBytes = Encoding.UTF8.GetBytes(testGetRequestString);

            var testRequest = new Request(
                "GET",
                "/",
                "HTTP/1.1",
                new Dictionary<string, string>
                {
                    {"FOO", "bar"}
                }
            );


            var testDataParser = new RequestParser();
            var result = testDataParser.Parse(testGetRequestBytes);


            Assert.True(result.Equals(testRequest));
        }
    }
}
