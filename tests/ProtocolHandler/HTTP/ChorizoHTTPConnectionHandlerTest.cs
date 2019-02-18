using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
        private readonly string _testGetRequestString;
        private readonly byte[] _testGetRequestBytes;
        private readonly string _testPostRequestAndBodyString;
        private readonly string _testPostRequestNoBodyString;
        private readonly byte[] _testPostRequestBytes;

        public ChorizoHttpConnectionHandlerTest()
        {
            _testGetRequestString = "GET / HTTP/1.1\r\n" +
                                    "cache-control: no-cache\r\n" +
                                    "Postman-Token: e6ff99e3-02c5-4c16-a4e7-0fb1896333c6\r\n" +
                                    "User-Agent: PostmanRuntime/7.4.0\r\n" +
                                    "Accept: */*\r\n" +
                                    "Host: localhost:5000\r\n" +
                                    "accept-encoding: gzip, deflate\r\n" +
                                    "\r\n";
            _testGetRequestBytes = Encoding.UTF8.GetBytes(_testGetRequestString);

            _testPostRequestAndBodyString = "POST /login HTTP/1.1\r\n" +
                                            "Content-Type: application/json\r\n" +
                                            "cache-control: no-cache\r\n" +
                                            "Postman-Token: 1686070e-07ef-4198-abef-3bc725324d49\r\n" +
                                            "User-Agent: PostmanRuntime/7.4.0\r\n" +
                                            "Accept: */*\r\n" +
                                            "Host: localhost:5000\r\n" +
                                            "accept-encoding: gzip, deflate\r\n" +
                                            "content-length: 68\r\n" +
                                            "\r\n" +
                                            "{ \"username\": \"testuser123\", \"password\": \"trulyterriblepassword\" }";
            _testPostRequestNoBodyString = "POST /login HTTP/1.1\r\n" +
                                           "Content-Type: application/json\r\n" +
                                           "cache-control: no-cache\r\n" +
                                           "Postman-Token: 1686070e-07ef-4198-abef-3bc725324d49\r\n" +
                                           "User-Agent: PostmanRuntime/7.4.0\r\n" +
                                           "Accept: */*\r\n" +
                                           "Host: localhost:5000\r\n" +
                                           "accept-encoding: gzip, deflate\r\n" +
                                           "content-length: 68\r\n" +
                                           "\r\n";
            _testPostRequestBytes = Encoding.UTF8.GetBytes(_testPostRequestAndBodyString);
        }

        [Fact]
        public void HandleRequestHandlesProperlyFormattedHttpRequestsWithoutBodies()
        {
            var testParsedData = new ParsedRequestData
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

            var testRequest = new Request(testParsedData, null);

            var mockSocket = new Mock<IChorizoSocket>();
            var byteCount = 0;
            mockSocket.Setup(sock => sock.Receive(It.IsAny<int>()))
                .Returns(() => new Tuple<byte[], int>(new[] {_testGetRequestBytes[byteCount++]}, 1));

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
            mockRequestParser.Verify(rp => rp.Parse(_testGetRequestString));
            mockRouter.Verify(route => route.Match(
                It.Is<Request>(req => req.Equals(testRequest)),
                It.Is<Response>(res => res.Equals(testResponse))
            ));
        }

        [Fact]
        public void HandleRequestHandlesProperlyFormattedHttpRequestsWithBodies()
        {
            var reqBodyBytes =
                Encoding.UTF8.GetBytes("{ \"username\": \"testuser123\", \"password\": \"trulyterriblepassword\" }");
            var testParsedData = new ParsedRequestData
            {
                Method = "POST",
                Path = "/login",
                Protocol = "HTTP/1.1",
                Headers = new Dictionary<string, string>
                {
                    {"CONTENT-TYPE", "application/json"},
                    {"CACHE-CONTROL", "no-cache"},
                    {"POSTMAN-TOKEN", "e6ff99e3-02c5-4c16-a4e7-0fb1896333c6"},
                    {"USER-AGENT", "PostmanRuntime/7.4.0"},
                    {"ACCEPT", "*/*"},
                    {"HOST", "localhost:5000"},
                    {"ACCEPT-ENCODING", "gzip, deflate"},
                    {"CONTENT-LENGTH", "68"}
                }
            };

            var testRequest = new Request(testParsedData, reqBodyBytes);

            var mockSocket = new Mock<IChorizoSocket>();
            var byteCount = 0;
            mockSocket.Setup(sock => sock.Receive(1))
                .Returns(() => new Tuple<byte[], int>(new[] {_testPostRequestBytes[byteCount++]}, 1));
            mockSocket.Setup(sock => sock.Receive(68))
                .Returns(() => new Tuple<byte[], int>(reqBodyBytes, 68));

            var testResponse = new Response(mockSocket.Object);

            var mockRequestParser = new Mock<IRequestParser>();
            mockRequestParser.Setup(rp => rp.Parse(It.IsAny<string>())).Returns(testParsedData);


            var mockRouter = new Mock<IRouter>();
            var testHandler = new ChorizoHTTPConnectionHandler(
                rawRequestParser: mockRequestParser.Object,
                router: mockRouter.Object
            );

            testHandler.HandleRequest(mockSocket.Object);

            mockSocket.Verify(sock => sock.Receive(1), Times.Exactly(255));
            mockRequestParser.Verify(rp => rp.Parse(_testPostRequestNoBodyString));
            mockRouter.Verify(route => route.Match(
                It.Is<Request>(req => req.Equals(testRequest)),
                It.Is<Response>(res => res.Equals(testResponse))
            ));
        }

        [Fact]
        public void
            HandleRequestClosesTheConnectionIfItRecievesEightThousandBytesBeforeItReceivesTwoConsecutiveCRLFs()
        {
            var mockSocket = new Mock<IChorizoSocket>();

            var fByte = Encoding.UTF8.GetBytes("F");
            mockSocket.Setup(sock => sock.Receive(It.IsAny<int>())).Returns(new Tuple<byte[], int>(fByte, 1));

            var mockRequestParser = new Mock<IRequestParser>();
            var mockRouter = new Mock<IRouter>();

            var testHandler = new ChorizoHTTPConnectionHandler(
                rawRequestParser: mockRequestParser.Object,
                router: mockRouter.Object
            );

            testHandler.HandleRequest(mockSocket.Object);

            mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both));
        }

        [Fact]
        public void HandleRequestWillCloseTheConnectionIfItReceivesZeroBytes()
        {
            var mockSocket = new Mock<IChorizoSocket>();
            var nullByte = new byte[0];
            mockSocket.Setup(sock => sock.Receive(It.IsAny<int>())).Returns(new Tuple<byte[], int>(nullByte, 0));

            var mockRequestParser = new Mock<IRequestParser>();
            var mockRouter = new Mock<IRouter>();

            var testHandler = new ChorizoHTTPConnectionHandler(
                rawRequestParser: mockRequestParser.Object,
                router: mockRouter.Object
            );

            testHandler.HandleRequest(mockSocket.Object);

            mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both));
        }
    }
}
