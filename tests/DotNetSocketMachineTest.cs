//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using Moq;
//using Xunit;
//
//namespace Chorizo.Tests
//{
//    public class DotNetSocketMachineTest
//    {
//        private readonly int _testPort;
//        private readonly string _testHostName;
//        private readonly Mock<ICzoSocket> _mockCzoSocket;
//        private readonly Mock<IRequestBuilder> _mockRequestBuilder;
//        private readonly string _testGetRequestString;
//        private readonly Request _testRequest;
//
//        public DotNetSocketMachineTest()
//        {
//            _testPort = 8000;
//            _testHostName = "localhost";
//            var testParams = new Dictionary<string, string>
//            {
//                {"cache-control", "no-cache"},
//                {"Postman-Token", "3f77f85c-b78e-46ef-94cc-a82b1cbacc86"},
//                {"User-Agent", "PostmanRuntime/7.4.0"},
//                {"Accept", "*/*"},
//                {"Host", "localhost:8000"},
//                {"accept-encoding", "gzip, deflate"},
//                {"Connection", "keep-alive"}
//            };
//            _testRequest = new Request("GET", "", "HTTP/1.1", testParams);
//            _mockCzoSocket = new Mock<ICzoSocket>();
//            _mockRequestBuilder = new Mock<IRequestBuilder>();
//            _testGetRequestString =
//                "GET  HTTP/1.1\r\nHost: localhost:8000\r\ncache-control: no-cache,no-cache\r\nPostman-Token: 7580f83c-1d96-4996-a58e-0395fc4296c4\r\nUser-Agent: PostmanRuntime/7.4.0\r\nAccept: */*\r\nHost: localhost:8000\r\naccept-encoding: gzip, deflate\r\nConnection: keep-alive\r\n\r\n";
//        }
//        [Fact]
//        public void ListenCreatesANewSocketAndStartsListeningOnIt()
//        {
//            var testSocketMachine = new DotNetSocketMachine
//            {
//                SocketImplementation = _mockCzoSocket.Object,
//                RequestBuilder = _mockRequestBuilder.Object
//            };
//            
//            testSocketMachine.Listen(_testPort, _testHostName);
//            
//            _mockCzoSocket.Verify(sock => sock.Bind(It.IsAny<IPEndPoint>()));
//            _mockCzoSocket.Verify(sock => sock.Listen(10));
//        }
//
//        [Fact]
//        public void AcceptConnection_AcceptConnectionsComingFromTheSocketAndReturnsARequestResponseTuple()
//        {
//            var testSpecificRequestBuilder = new Mock<IRequestBuilder>();
//            testSpecificRequestBuilder.Setup(bob => bob.Build(It.IsAny<string>())).Returns(_testRequest);
//            var mockAcceptedSocket = new Mock<ICzoSocket>();
//            var testBytes = Encoding.UTF8.GetBytes(_testGetRequestString);
//            mockAcceptedSocket.Setup(sock => sock.Receive(It.IsAny<int>())).Returns(new Tuple<byte[], int>(testBytes, 251));
//            _mockCzoSocket.Setup(sock => sock.Accept()).Returns(mockAcceptedSocket.Object);
//            var testSocketMachine = new DotNetSocketMachine
//            {
//                SocketImplementation = _mockCzoSocket.Object,
//                RequestBuilder = testSpecificRequestBuilder.Object
//            };
//            
//            var result = testSocketMachine.AcceptConnection();
//            
//            _mockCzoSocket.Verify(sock => sock.Accept());
//            testSpecificRequestBuilder.Verify(bob => bob.Build(It.IsAny<string>()));
//            Assert.IsType<Tuple<Request, Response>>(result);
//        }
//        
//        [Fact]
//        public void GetDataGetsARequestWithNoBodyAndReturnsTheHeaderAsAStringAndAnEmptyByteArray()
//        {
//            var mockAcceptedSocket = new Mock<ICzoSocket>();
//            var testSocketMachine = new DotNetSocketMachine
//            {
//                SocketImplementation = _mockCzoSocket.Object,
//                RequestBuilder = _mockRequestBuilder.Object
//            };
//
//            var testBytes = Encoding.UTF8.GetBytes(_testGetRequestString);
//            
//            mockAcceptedSocket.Setup(sock => sock.Receive(It.IsAny<int>())).Returns(new Tuple<byte[], int>(testBytes, 251));
//
//            var result = testSocketMachine.GetHeader(mockAcceptedSocket.Object);
//            var (header, body) = result;
//            
//            mockAcceptedSocket.Verify(sock => sock.Receive(It.IsAny<int>()));
//            Assert.IsType<Tuple<string, byte[]>>(result);
//            Assert.Equal(header, _testGetRequestString);
//            Assert.Equal(new byte[0], body);
//        }
//
//        [Fact]
//        public void
//            GetDataGetsARequestWithABodyAndReturnsTheHeaderAsAStringAndByteArrayContainingAnyBytesOfTheBodyThatWereReceivedWhenReceivingTheHeader()
//        {
//            var mockAcceptedSocket = new Mock<ICzoSocket>();
//            var testSocketMachine = new DotNetSocketMachine
//            {
//                SocketImplementation = _mockCzoSocket.Object,
//                RequestBuilder = _mockRequestBuilder.Object
//            };
//
//            const string testRequest =
//                "POST  HTTP/1.1\r\nHost: localhost:8000\r\ncache-control: no-cache\r\nUser-Agent: PostmanRuntime/7.4.0\r\nAccept: */*\r\nHost: localhost:8000\r\naccept-encoding: gzip, deflate\r\nContent-Length: 101\r\nConnection: keep-alive\r\nContent-Type: application/json\r\nPostman-Token: 93855cbb-fee7-4d6a-b981-394c0f3066c4\r\n\r\n{\r\n\t\"test1\": \"Hello World!\",\r\n\t\"test2\": 1337,\r\n\t\"test3\": {\r\n\t\t\"subtest1\": \"foo\",\r\n\t\t\"subtest2\": \"bar\"\r\n\t}\r\n}";
//            const string testHeaderOnly =
//                "POST  HTTP/1.1\r\nHost: localhost:8000\r\ncache-control: no-cache\r\nUser-Agent: PostmanRuntime/7.4.0\r\nAccept: */*\r\nHost: localhost:8000\r\naccept-encoding: gzip, deflate\r\nContent-Length: 101\r\nConnection: keep-alive\r\nContent-Type: application/json\r\nPostman-Token: 93855cbb-fee7-4d6a-b981-394c0f3066c4\r\n\r\n";
//            const string testBodyOnly =
//                "{\r\n\t\"test1\": \"Hello World!\",\r\n\t\"test2\": 1337,\r\n\t\"test3\": {\r\n\t\t\"subtest1\": \"foo\",\r\n\t\t\"subtest2\": \"bar\"\r\n\t}\r\n}";
//            var testBytesFull = Encoding.UTF8.GetBytes(testRequest);
//            var ptOne = new byte[256];
//            Buffer.BlockCopy(testBytesFull, 0, ptOne, 0, 256);
//            var ptTwo = new byte[256];
//            Buffer.BlockCopy(testBytesFull, 256, ptTwo, 0, testBytesFull.Length - 256);
//            mockAcceptedSocket.SetupSequence(sock => sock.Receive(It.IsAny<int>()))
//                .Returns(new Tuple<byte[], int>(ptOne, 256))
//                .Returns(new Tuple<byte[], int>(ptTwo, testBytesFull.Length - 256));
//
//            var result = testSocketMachine.GetHeader(mockAcceptedSocket.Object);
//            var (header, body) = result;
//            var bodyText = Encoding.UTF8.GetString(body);
//
//            // interface Stream {
//            // next(), take(10), takeAll()
//        // }
//
//        Assert.IsType<Tuple<string, byte[]>>(result);
//            Assert.Equal(header, testHeaderOnly);
//            Assert.Equal(bodyText, testBodyOnly);
//        }
//    }
//}