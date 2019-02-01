using System;
using System.Net.Sockets;
using Chorizo.ProtocolHandler;
using Moq;
using Xunit;

namespace Chorizo.Tests
{
    public class CzoTelNetHandlerTest
    {
        [Fact]
        public void WillHandleReturnsTrueIfItCanHandleTheProtocol()
        {
            var testHandler = new CzoTelNetHandler();

            Assert.True(testHandler.WillHandle("TelNet"));
        }

        [Fact]
        public void WillHandleReturnsFalseIfItCannotHandleTheProtocol()
        {
            var testHandler = new CzoTelNetHandler();

            Assert.False(testHandler.WillHandle("HTTP"));
        }

        [Fact]
        public void HandleEchosTheDataSentFromTheClientBackToItself()
        {
            
            var testHandler = new CzoTelNetHandler();
            var testNums = new byte[] {70, 79, 79, 10};
            var mockCzoSocket = new Mock<ICzoSocket>();
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