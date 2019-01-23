using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Moq;
using Xunit;

namespace Chorizo.Tests
{
    public class DotNetSocketMachineTest
    {
        private readonly int _testPort;
        private readonly string _testHostName;
        private readonly Mock<ICzoSocket> _mockCzoSocket;
        private readonly Mock<IRequestBuilder> _mockRequestBuilder;

        public DotNetSocketMachineTest()
        {
            _testPort = 8000;
            _testHostName = "localhost";
            _mockCzoSocket = new Mock<ICzoSocket>();
            _mockRequestBuilder = new Mock<IRequestBuilder>();
        }
        [Fact]
        public void ListenCreatesANewSocketAndStartsListeningOnIt()
        {
            var testSocketMachine = new DotNetSocketMachine
            {
                SocketImplementation = _mockCzoSocket.Object,
                RequestBuilder = _mockRequestBuilder.Object
            };
            
            testSocketMachine.Listen(_testPort, _testHostName);
            
            _mockCzoSocket.Verify(sock => sock.Bind(It.IsAny<IPEndPoint>()));
            _mockCzoSocket.Verify(sock => sock.Listen(_testPort));
        }

        [Fact]
        public void AcceptConnection_AcceptConnectionsComingFromTheSocketAndReturnsARequestResponseTuple()
        {
            var testSpecificRequestBuilder = new Mock<IRequestBuilder>();
            testSpecificRequestBuilder.Setup(bob => bob.Build(It.IsAny<string>())).Returns(new Request());
            var testSocketMachine = new DotNetSocketMachine
            {
                SocketImplementation = _mockCzoSocket.Object,
                RequestBuilder = testSpecificRequestBuilder.Object
            };

            var result = testSocketMachine.AcceptConnection();
            
            _mockCzoSocket.Verify(sock => sock.Accept());
            testSpecificRequestBuilder.Verify(bob => bob.Build(It.IsAny<string>()));
            Assert.IsType<Tuple<Request, Response>>(result);
        }
        
        [Fact]
        public void GetDataGetsARequestWithNoBodyAndReturnsTheHeaderAsAStringAndAnEmptyByteArray()
        {
            var testSocketMachine = new DotNetSocketMachine
            {
                SocketImplementation = _mockCzoSocket.Object,
                RequestBuilder = _mockRequestBuilder.Object
            };

            const string testString =
                "GET  HTTP/1.1\r\nHost: localhost:8000\r\ncache-control: no-cache,no-cache\r\nPostman-Token: 7580f83c-1d96-4996-a58e-0395fc4296c4\r\nUser-Agent: PostmanRuntime/7.4.0\r\nAccept: */*\r\nHost: localhost:8000\r\naccept-encoding: gzip, deflate\r\nConnection: keep-alive\r\n\r\n";
            
            var testBytes = Encoding.UTF8.GetBytes(testString);
            
            _mockCzoSocket.Setup(sock => sock.Receive(It.IsAny<byte[]>())).Returns(new Tuple<byte[], int>(testBytes, 251));

            var result = testSocketMachine.GetData();
            var (header, body) = result;
            
            _mockCzoSocket.Verify(sock => sock.Receive(It.IsAny<byte[]>()));
            Assert.IsType<Tuple<string, byte[]>>(result);
            Assert.Equal(header, testString);
            Assert.Equal(new byte[0], body);
        }

        [Fact]
        public void GetDataGetsARequestWithABodyAndReturnsTheHeaderAsAStringAndByteArrayContainingAnyBytesOfTheBodyThatWereReceivedWhenReceivingTheHeader()
        {
            var testSocketMachine = new DotNetSocketMachine
            {
                SocketImplementation = _mockCzoSocket.Object,
                RequestBuilder = _mockRequestBuilder.Object
            };
            
            const string testRequest =
                "POST  HTTP/1.1\r\nHost: localhost:8000\r\ncache-control: no-cache\r\nUser-Agent: PostmanRuntime/7.4.0\r\nAccept: */*\r\nHost: localhost:8000\r\naccept-encoding: gzip, deflate\r\nContent-Length: 101\r\nConnection: keep-alive\r\nContent-Type: application/json\r\nPostman-Token: 93855cbb-fee7-4d6a-b981-394c0f3066c4\r\n\r\n{\r\n\t\"test1\": \"Hello World!\",\r\n\t\"test2\": 1337,\r\n\t\"test3\": {\r\n\t\t\"subtest1\": \"foo\",\r\n\t\t\"subtest2\": \"bar\"\r\n\t}\r\n}";
            const string testHeaderOnly =
                "POST  HTTP/1.1\r\nHost: localhost:8000\r\ncache-control: no-cache\r\nUser-Agent: PostmanRuntime/7.4.0\r\nAccept: */*\r\nHost: localhost:8000\r\naccept-encoding: gzip, deflate\r\nContent-Length: 101\r\nConnection: keep-alive\r\nContent-Type: application/json\r\nPostman-Token: 93855cbb-fee7-4d6a-b981-394c0f3066c4\r\n\r\n";
            const string testBodyOnly =
                "{\r\n\t\"test1\": \"Hello World!\",\r\n\t\"test2\": 1337,\r\n\t\"test3\": {\r\n\t\t\"subtest1\": \"foo\",\r\n\t\t\"subtest2\": \"bar\"\r\n\t}\r\n}";
            var testBytesFull = Encoding.UTF8.GetBytes(testRequest);
            var ptOne = new byte[256];
            Buffer.BlockCopy(testBytesFull, 0, ptOne, 0, 256);
            var ptTwo = new byte[256];
            Buffer.BlockCopy(testBytesFull, 256, ptTwo, 0, testBytesFull.Length - 256);
            _mockCzoSocket.SetupSequence(sock => sock.Receive(It.IsAny<byte[]>()))
                .Returns(new Tuple<byte[], int>(ptOne, 256))
                .Returns(new Tuple<byte[], int>(ptTwo, testBytesFull.Length - 256));
            
            var result = testSocketMachine.GetData();
            var (header, body) = result;
            var bodyText = Encoding.UTF8.GetString(body);
            
            Assert.IsType<Tuple<string, byte[]>>(result);
            Assert.Equal(header, testHeaderOnly);
            Assert.Equal(bodyText, testBodyOnly);
        }
    }
}