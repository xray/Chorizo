using System;
using System.Net.Sockets;
using Chorizo.ProtocolHandler;
using Chorizo.Sockets.CzoSocket;
using Moq;
using Xunit;

namespace Chorizo.Tests
{
    public class ChorizoEchoHandlerTest
    {
        [Fact]
        public void WillHandleReturnsTrueIfItCanHandleTheProtocol()
        {
            var testHandler = new ChorizoEchoHandler();

            Assert.True(testHandler.WillHandle("TelNet"));
        }

        [Fact]
        public void WillHandleReturnsFalseIfItCannotHandleTheProtocol()
        {
            var testHandler = new ChorizoEchoHandler();

            Assert.False(testHandler.WillHandle("HTTP"));
        }

        [Fact]
        public void HandleEchosTheDataSentFromTheClientBackToItself()
        {
            
            var testHandler = new ChorizoEchoHandler();
            var testNums = new byte[] {70, 79, 79, 10};
            var mockCzoSocket = new Mock<IChorizoSocket>();
            mockCzoSocket.SetupSequence(sock => sock.Receive(It.IsAny<int>()))
                .Returns(new Tuple<byte[], int>(new byte[] {70}, 1))
                .Returns(new Tuple<byte[], int>(new byte[] {79}, 1))
                .Returns(new Tuple<byte[], int>(new byte[] {79}, 1))
                .Returns(new Tuple<byte[], int>(new byte[] {10}, 1));
            
            testHandler.Handle(mockCzoSocket.Object);
            
            mockCzoSocket.Verify(sock => sock.Receive(It.IsAny<int>()), Times.Exactly(4));
            mockCzoSocket.Verify(sock => sock.Send(testNums));
            mockCzoSocket.Verify(sock => sock.Shutdown(SocketShutdown.Both));
            mockCzoSocket.Verify(sock => sock.Close());
        }
    }
}