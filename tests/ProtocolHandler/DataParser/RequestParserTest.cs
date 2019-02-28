using System;
using System.Text;
using Chorizo.ProtocolHandler.DataParser;
using Chorizo.ProtocolHandler.HttpHeaders;
using Chorizo.ProtocolHandler.ResponseRetriever;
using Xunit;
using Xunit.Abstractions;

namespace Chorizo.Tests.ProtocolHandler.DataParser
{
    public class RequestParserTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RequestParserTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

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
                new Headers().AddHeader("foo", "bar")
            );


            var testDataParser = new RequestParser();
            var result = testDataParser.Parse(testGetRequestBytes);


            Assert.True(testRequest.Equals(result));
        }
    }
}
