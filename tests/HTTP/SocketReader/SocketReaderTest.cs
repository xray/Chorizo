using System;
using System.Text;
using Chorizo.Sockets.CzoSocket;
using Moq;
using Xunit;

namespace Chorizo.Tests.HTTP.SocketReader
{
    public class SocketReaderTest
    {
        [Fact]
        public void ReadSocketReceivesBytesFromTheSocketUntilItReceivesTwoConsecutiveCRLFs()
        {
            var testGetRequestString = "GET / HTTP/1.1\r\n" +
                                       "foo: bar\r\n" +
                                       "\r\n";
            var testGetRequestBytes = Encoding.UTF8.GetBytes(testGetRequestString);

            var mockSocket = new Mock<IChorizoSocket>();
            var byteCount = 0;
            mockSocket.Setup(sock => sock.Receive(It.IsAny<int>()))
                .Returns(() => new Tuple<byte[], int>(new[] {testGetRequestBytes[byteCount++]}, 1));

            var testSocketReader = new global::Chorizo.HTTP.SocketReader.SocketReader();


            Assert.Equal(testSocketReader.ReadSocket(mockSocket.Object), testGetRequestBytes);
            mockSocket.Verify(sock => sock.Receive(1), Times.Exactly(28));
        }
    }
}
