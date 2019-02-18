using System;
using System.Net.Sockets;
using System.Text;
using Chorizo.Date;
using Chorizo.ProtocolHandler.HTTP.Responses;
using Chorizo.Sockets.CzoSocket;
using Moq;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.HTTP.Responses
{
    public class ResponseTest
    {
        private readonly Mock<IChorizoSocket> _mockSocket;
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
        private readonly DateTime _testTime;
        private readonly Response _testResponse;

        public ResponseTest()
        {
            _testTime = new DateTime(1997, 12, 02, 15, 10, 00, DateTimeKind.Utc);
            _mockSocket = new Mock<IChorizoSocket>();
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
            _mockDateTimeProvider.Setup(dt => dt.Now()).Returns(_testTime);
            _testResponse = new Response(_mockSocket.Object, _mockDateTimeProvider.Object);
        }

        [Fact]
        public void SendWillSendAProperlyFormattedResponseViaTheSocketAndThenDisconnect()
        {
            var checkResponseString = "HTTP/1.1 200 OK\r\n" +
                                      "Connection: Closed\r\n" +
                                      "date: Tue, 02 Dec 1997 15:10:00 GMT\r\n" +
                                      "Server: Chorizo\r\n" +
                                      "\r\n";
            var checkResponseBytes = Encoding.UTF8.GetBytes(checkResponseString);

            _testResponse.Send();

            _mockDateTimeProvider.Verify(dtp => dtp.Now(), Times.Once);
            _mockSocket.Verify(sock => sock.Send(checkResponseBytes), Times.Once);
            _mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both), Times.Once);
        }

        [Fact]
        public void SendWillSendAProperlyFormattedResponseAlongWithThePassedMessageViaTheSocketThenDisconnect()
        {
            var checkResponseString = "HTTP/1.1 200 OK\r\n" +
                                      "Connection: Closed\r\n" +
                                      "date: Tue, 02 Dec 1997 15:10:00 GMT\r\n" +
                                      "Server: Chorizo\r\n" +
                                      "Content-Length: 2\r\n" +
                                      "Content-Type: text/html\r\n" +
                                      "\r\n" +
                                      "OK";
            var checkResponseBytes = Encoding.UTF8.GetBytes(checkResponseString);

            _testResponse.Send("OK");

            _mockDateTimeProvider.Verify(dtp => dtp.Now(), Times.Once);
            _mockSocket.Verify(sock => sock.Send(checkResponseBytes), Times.Once);
            _mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both), Times.Once);
        }

        [Fact]
        public void StatusWillUpdateTheResponseCodeAndTextThatWillBeSent()
        {
            var checkResponseString = "HTTP/1.1 404 Not Found\r\n" +
                                      "Connection: Closed\r\n" +
                                      "date: Tue, 02 Dec 1997 15:10:00 GMT\r\n" +
                                      "Server: Chorizo\r\n" +
                                      "\r\n";
            var checkResponseBytes = Encoding.UTF8.GetBytes(checkResponseString);

            _testResponse.Status(404).Send();

            _mockDateTimeProvider.Verify(dtp => dtp.Now(), Times.Once);
            _mockSocket.Verify(sock => sock.Send(checkResponseBytes), Times.Once);
            _mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both), Times.Once);
        }

        [Fact]
        public void AppendWillAddANewHeaderToTheRequestThatWillBeSent()
        {
            var checkResponseString = "HTTP/1.1 200 OK\r\n" +
                                      "Connection: Closed\r\n" +
                                      "date: Tue, 02 Dec 1997 15:10:00 GMT\r\n" +
                                      "Server: Chorizo\r\n" +
                                      "foo: bar\r\n" +
                                      "\r\n";
            var checkResponseBytes = Encoding.UTF8.GetBytes(checkResponseString);

            _testResponse.Append("foo", "bar").Send();

            _mockDateTimeProvider.Verify(dtp => dtp.Now(), Times.Once);
            _mockSocket.Verify(sock => sock.Send(checkResponseBytes), Times.Once);
            _mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both), Times.Once);
        }
    }
}
