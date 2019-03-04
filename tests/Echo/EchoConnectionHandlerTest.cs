using System;
using System.Net.Sockets;
using Chorizo.Echo;
using Chorizo.Sockets.CzoSocket;
using Moq;
using Xunit;

namespace Chorizo.Tests.Echo
{
    public class EchoConnectionHandlerTest
    {
        [Fact]
        public void HandleEchosTheDataSentFromTheClientBackToItself()
        {
            var testHandler = new EchoConnectionHandler();
            var testNums = new byte[] {70, 79, 79, 10};
            var mockCzoSocket = new Mock<IChorizoSocket>();
            mockCzoSocket.SetupSequence(sock => sock.Receive(It.IsAny<int>()))
                .Returns(new Tuple<byte[], int>(new byte[] {70}, 1))
                .Returns(new Tuple<byte[], int>(new byte[] {79}, 1))
                .Returns(new Tuple<byte[], int>(new byte[] {79}, 1))
                .Returns(new Tuple<byte[], int>(new byte[] {10}, 1));

            testHandler.HandleRequest(mockCzoSocket.Object);

            mockCzoSocket.Verify(sock => sock.Receive(It.IsAny<int>()), Times.Exactly(4));
            mockCzoSocket.Verify(sock => sock.Send(testNums));
            mockCzoSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both));
        }
    }
}
