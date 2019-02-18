using System.Collections.Generic;
using Chorizo.ProtocolHandler.HTTP.Requests;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.HTTP.Requests
{
    public class RawRequestParserTest
    {
        private const string TestGetRequestString = "GET / HTTP/1.1\r\n" +
                                                    "cache-control: no-cache\r\n" +
                                                    "Postman-Token: e6ff99e3-02c5-4c16-a4e7-0fb1896333c6\r\n" +
                                                    "User-Agent: PostmanRuntime/7.4.0\r\n" +
                                                    "Accept: */*\r\n" +
                                                    "Host: localhost:5000\r\n" +
                                                    "accept-encoding: gzip, deflate\r\n" +
                                                    "\r\n";

        [Fact]
        public void ParseShouldReturnAParsedRequestDataWhenGivenAValidHTTPRequest()
        {
            var testParsedRequest = new ParsedRequestData
            {
                Method = "GET",
                Path = "/",
                Protocol = "HTTP/1.1",
                Headers = new Dictionary<string, string>
                {
                    {"CACHE-CONTROL", "no-cache"},
                    {"POSTMAN-TOKEN", "e6ff99e3-02c5-4c16-a4e7-0fb1896333c6"},
                    {"USER-AGENT", "PostmanRuntime/7.4.0"},
                    {"ACCEPT", "*/*"},
                    {"HOST", "localhost:5000"},
                    {"ACCEPT-ENCODING", "gzip, deflate"}
                }
            };

            var testRequestParser = new RawRequestParser();
            var parsedData = testRequestParser.Parse(TestGetRequestString);

            Assert.True(testParsedRequest.Equals(parsedData));
        }
    }
}
