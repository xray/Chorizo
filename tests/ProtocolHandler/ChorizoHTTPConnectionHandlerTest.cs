using System.Net.Sockets;
using System.Text;
using Chorizo.ProtocolHandler;
using Chorizo.ProtocolHandler.DataParser;
using Chorizo.ProtocolHandler.ResponseRetriever;
using Chorizo.ProtocolHandler.SocketReader;
using Chorizo.Sockets.CzoSocket;
using Xunit;
using Moq;

namespace Chorizo.Tests.ProtocolHandler
{
    public class ChorizoHTTPConnectionHandlerTest
    {
        private readonly ChorizoHttpConnectionHandler _testConnectionHandler;
        private readonly Mock<IChorizoSocket> _mockSocket;
        private readonly Mock<IHTTPSocketReader> _mockSocketReader;
        private readonly Mock<IDataParser> _mockDataParser;
        private readonly Mock<IResponseRetriever> _mockResponseRetriever;
        private readonly string _testGetRequestString;
        private readonly byte[] _testGetRequestBytes;
        private readonly Request _testGetRequest;
        private Response _testGetResponse;

        public ChorizoHTTPConnectionHandlerTest()
        {
            _mockSocket = new Mock<IChorizoSocket>();
            _mockSocketReader = new Mock<IHTTPSocketReader>();
            _mockDataParser = new Mock<IDataParser>();
            _mockResponseRetriever = new Mock<IResponseRetriever>();

            _testConnectionHandler = new ChorizoHttpConnectionHandler()
            {
                HttpSocketReader = _mockSocketReader.Object,
                DataParser = _mockDataParser.Object,
                ResponseRetriever = _mockResponseRetriever.Object
            };

            _testGetRequestString = "GET / HTTP/1.1\r\n" +
                                    "\r\n";
            _testGetRequestBytes = Encoding.UTF8.GetBytes(_testGetRequestString);

            _testGetRequest = new Request(
                "GET",
                "/",
                "HTTP/1.1"
            );

            _testGetResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("fake", "header");

            _mockResponseRetriever.Setup(rr => rr.Retrieve(It.IsAny<Request>()))
                .Returns(_testGetResponse);
        }

        [Fact]
        public void HandleRequestTakesInAnIChorizoSocketAndReadsDataFromTheSocket()
        {
            _testConnectionHandler.HandleRequest(_mockSocket.Object);

            _mockSocketReader.Verify(sr => sr.ReadSocket(_mockSocket.Object));
        }

        [Fact]
        public void HandleRequestUsesDataReadAndParsesTheData()
        {
            _mockSocketReader.Setup(sr => sr.ReadSocket(It.IsAny<IChorizoSocket>()))
                .Returns(_testGetRequestBytes);

            _testConnectionHandler.HandleRequest(_mockSocket.Object);

            _mockDataParser.Verify(dp => dp.Parse(_testGetRequestBytes));
        }

        [Fact]
        public void HandleRequestUsesRequestToGetAResponse()
        {
            _mockSocketReader.Setup(sr => sr.ReadSocket(It.IsAny<IChorizoSocket>()))
                .Returns(_testGetRequestBytes);

            _mockDataParser.Setup(dp => dp.Parse(It.IsAny<byte[]>()))
                .Returns(_testGetRequest);

            _testConnectionHandler.HandleRequest(_mockSocket.Object);

            _mockResponseRetriever.Verify(rr => rr.Retrieve(_testGetRequest));
        }

        [Fact]
        public void HandleRequestUsesResponseToSendOnTheSocket()
        {
            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("fake", "header");

            _mockSocketReader.Setup(sr => sr.ReadSocket(It.IsAny<IChorizoSocket>()))
                .Returns(_testGetRequestBytes);

            _mockDataParser.Setup(dp => dp.Parse(It.IsAny<byte[]>()))
                .Returns(_testGetRequest);

            _mockResponseRetriever.Setup(rr => rr.Retrieve(It.IsAny<Request>()))
                .Returns(testResponse);

            _testConnectionHandler.HandleRequest(_mockSocket.Object);

            var testBytes = testResponse.ToByteArray();

            _mockSocket.Verify(sock => sock.Send(testBytes));
            _mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both));
        }
    }
}
