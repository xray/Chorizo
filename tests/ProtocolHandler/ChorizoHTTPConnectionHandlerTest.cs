using System.Collections.Generic;
using System.Text;
using Chorizo.ProtocolHandler;
using Chorizo.ProtocolHandler.SocketReader;
using Chorizo.Sockets.CzoSocket;
using Xunit;
using Moq;

namespace Chorizo.Tests.ProtocolHandler
{
    public class ChorizoHTTPConnectionHandlerTest
    {
        private readonly ChorizoHTTPConnectionHandler _testConnectionHandler;
        private readonly Mock<IChorizoSocket> _mockSocket;
        private readonly Mock<IHTTPSocketReader> _mockSocketReader;
        private readonly Mock<IDataParser> _mockDataParser;
        private readonly Mock<IResponseRetriever> _mockResponseRetriever;
        private readonly Mock<IResponseSender> _mockResponseSender;
        private readonly string _testGetRequestString;
        private readonly byte[] _testGetRequestBytes;
        private readonly Request _testGetRequest;

        public ChorizoHTTPConnectionHandlerTest()
        {
            _mockSocket = new Mock<IChorizoSocket>();
            _mockSocketReader = new Mock<IHTTPSocketReader>();
            _mockDataParser = new Mock<IDataParser>();
            _mockResponseRetriever = new Mock<IResponseRetriever>();
            _mockResponseSender = new Mock<IResponseSender>();

            _testConnectionHandler = new ChorizoHTTPConnectionHandler()
            {
                HttpSocketReader = _mockSocketReader.Object,
                DataParser = _mockDataParser.Object,
                ResponseRetriever = _mockResponseRetriever.Object,
                ResponseSender = _mockResponseSender.Object
            };

            _testGetRequestString = "GET / HTTP/1.1\r\n" +
                                    "foo: bar\r\n" +
                                    "\r\n";
            _testGetRequestBytes = Encoding.UTF8.GetBytes(_testGetRequestString);

            _testGetRequest = new Request(
                "GET",
                "/",
                "HTTP/1.1",
                new Dictionary<string, string>
                {
                    {"foo", "bar"}
                }
            );
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
        public void HandleRequestUsesResponseToWriteToTheSocket()
        {
            var testGetResponse = new Response(
                "HTTP/1.1",
                200,
                "OK",
                new Dictionary<string, string>
                {
                    {"fake", "header"}
                }
            );
            _mockSocketReader.Setup(sr => sr.ReadSocket(It.IsAny<IChorizoSocket>()))
                .Returns(_testGetRequestBytes);

            _mockDataParser.Setup(dp => dp.Parse(It.IsAny<byte[]>()))
                .Returns(_testGetRequest);

            _mockResponseRetriever.Setup(rr => rr.Retrieve(It.IsAny<Request>()))
                .Returns(testGetResponse);

            _testConnectionHandler.HandleRequest(_mockSocket.Object);

            _mockResponseSender.Verify(rs => rs.Send(_mockSocket.Object, testGetResponse));
        }
    }
}
