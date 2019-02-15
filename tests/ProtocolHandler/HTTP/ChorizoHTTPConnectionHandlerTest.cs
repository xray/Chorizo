using System;
using System.Collections.Generic;
using System.Text;
using Chorizo.ProtocolHandler.HTTP;
using Chorizo.ProtocolHandler.HTTP.Requests;
using Chorizo.ProtocolHandler.HTTP.Responses;
using Chorizo.ProtocolHandler.HTTP.Router;
using Chorizo.Sockets.CzoSocket;
using Moq;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.HTTP
{
    public class ChorizoHttpConnectionHandlerTest
    {
        [Fact]
        public void HandleRequestHandlesAnyProperlyFormattedHttpRequest()
        {
            const string testRequestString = "GET / HTTP/1.1\r\n" +
                                             "cache-control: no-cache\r\n" +
                                             "Postman-Token: e6ff99e3-02c5-4c16-a4e7-0fb1896333c6\r\n" +
                                             "User-Agent: PostmanRuntime/7.4.0\r\n" +
                                             "Accept: */*\r\n" +
                                             "Host: localhost:5000\r\n" +
                                             "accept-encoding: gzip, deflate\r\n" +
                                             "\r\n";

            var testRequestBytes = Encoding.UTF8.GetBytes(testRequestString);

            var testParsedData = new ParsedRequestData
            {
                Method = "GET",
                Path = "/",
                Protocol = "HTTP/1.1",
                Headers = new Dictionary<string, string>
                {
                    {"cache-control", "no-cache"},
                    {"Postman-Token", "e6ff99e3-02c5-4c16-a4e7-0fb1896333c6"},
                    {"User-Agent", "PostmanRuntime/7.4.0"},
                    {"Accept", "*/*"},
                    {"Host", "localhost:5000"},
                    {"accept-encoding", "gzip, deflate"}
                }
            };

            var testRequest = new Request(testParsedData, null);

            var mockSocket = new Mock<IChorizoSocket>();
            var byteCount = 0;
            mockSocket.Setup(sock => sock.Receive(It.IsAny<int>()))
                .Returns(() => new Tuple<byte[], int>(new[] {testRequestBytes[byteCount++]}, 1));

            var testResponse = new Response(mockSocket.Object);

            var mockRequestParser = new Mock<IRequestParser>();
            mockRequestParser.Setup(rp => rp.Parse(It.IsAny<string>())).Returns(testParsedData);

            var mockRouter = new Mock<IRouter>();
            var testHandler = new ChorizoHTTPConnectionHandler(
                rawRequestParser: mockRequestParser.Object,
                router: mockRouter.Object
            );

            testHandler.HandleRequest(mockSocket.Object);

            mockSocket.Verify(sock => sock.Receive(1), Times.Exactly(197));
            mockRequestParser.Verify(rp => rp.Parse(testRequestString));
            mockRouter.Verify(route => route.Match(
                It.Is<Request>(req => req.Equals(testRequest)),
                It.Is<Response>(res => res.Equals(testResponse))
            ));
        }
    }
}
