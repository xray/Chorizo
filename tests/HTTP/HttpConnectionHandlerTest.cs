using System.Net.Sockets;
using System.Text;
using Chorizo.HTTP;
using Chorizo.HTTP.DataParser;
using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.ReqProcessor;
using Chorizo.HTTP.SocketReader;
using Chorizo.Sockets.CzoSocket;
using Moq;
using Xunit;

namespace Chorizo.Tests.HTTP
{
    public class HttpConnectionHandlerTest
    {
        private readonly HttpConnectionHandler _testConnectionHandler;
        private readonly Mock<IChorizoSocket> _mockSocket;
        private readonly Mock<ISocketReader> _mockSocketReader;
        private readonly Mock<IDataParser> _mockDataParser;
        private readonly Mock<IRequestProcessor> _mockResponseRetriever;
        private readonly string _testGetRequestString;
        private readonly byte[] _testGetRequestBytes;
        private readonly Request _testGetRequest;
        private Response _testGetResponse;

        public HttpConnectionHandlerTest()
        {
            _mockSocket = new Mock<IChorizoSocket>();
            _mockSocketReader = new Mock<ISocketReader>();
            _mockDataParser = new Mock<IDataParser>();
            _mockResponseRetriever = new Mock<IRequestProcessor>();

            _testConnectionHandler = new HttpConnectionHandler()
            {
                SocketReader = _mockSocketReader.Object,
                DataParser = _mockDataParser.Object,
                RequestProcessor = _mockResponseRetriever.Object
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

            _mockResponseRetriever.Setup(rr => rr.Process(It.IsAny<Request>()))
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
            _mockSocketReader.Setup(sr => sr.ReadBody(It.IsAny<IChorizoSocket>(), It.IsAny<Request>()))
                .Returns(_testGetRequest);

            _mockDataParser.Setup(dp => dp.Parse(It.IsAny<byte[]>()))
                .Returns(_testGetRequest);

            _testConnectionHandler.HandleRequest(_mockSocket.Object);

            _mockResponseRetriever.Verify(rr => rr.Process(_testGetRequest));
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

            _mockResponseRetriever.Setup(rr => rr.Process(It.IsAny<Request>()))
                .Returns(testResponse);

            _testConnectionHandler.HandleRequest(_mockSocket.Object);

            var testBytes = testResponse.ToByteArray();

            _mockSocket.Verify(sock => sock.Send(testBytes));
            _mockSocket.Verify(sock => sock.Disconnect(SocketShutdown.Both));
        }
    }
}
