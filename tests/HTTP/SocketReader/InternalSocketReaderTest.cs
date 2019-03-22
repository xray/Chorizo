using System;
using System.Text;
using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.SocketReader;
using Chorizo.Sockets.InternalSocket;
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

            var mockSocket = new Mock<IAppSocket>();
            var byteCount = 0;
            mockSocket.Setup(sock => sock.Receive(It.IsAny<int>()))
                .Returns(() => new Tuple<byte[], int>(new[] {testGetRequestBytes[byteCount++]}, 1));

            var testSocketReader = new InternalSocketReader();


            Assert.Equal(testSocketReader.ReadSocket(mockSocket.Object), testGetRequestBytes);
            mockSocket.Verify(sock => sock.Receive(1), Times.Exactly(28));
        }

        [Fact]
        public void ReadBodyTakesInARequestWithAContentLengthHeaderAndReturnsANewRequestWithABody()
        {
            var testBody = Encoding.UTF8.GetBytes("some body");

            var testHeaders = new Headers()
                .AddHeader("Content-Length", "9");
            var testRequest = new Request("POST", "/echo_body", "HTTP/1.1", testHeaders);

            var mockSocket = new Mock<IAppSocket>();
            mockSocket.Setup(sock => sock.Receive(It.IsAny<int>()))
                .Returns(new Tuple<byte[], int>(testBody, 9));

            var result = new InternalSocketReader().ReadBody(mockSocket.Object, testRequest);

            var assertionRequest = new Request("POST", "/echo_body", "HTTP/1.1", testHeaders, "some body");

            Assert.True(assertionRequest.Equals(result));
        }

        [Fact]
        public void ReadBodyTakesInARequestWithNoContentLengthHeaderAndReturnsThePassedRequest()
        {
            var testRequest = new Request("POST", "/echo_body", "HTTP/1.1");

            var mockSocket = new Mock<IAppSocket>();

            var result = new InternalSocketReader().ReadBody(mockSocket.Object, testRequest);

            Assert.Equal(testRequest, result);
        }
    }
}
