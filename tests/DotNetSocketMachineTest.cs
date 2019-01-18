using System.Net;
using System.Net.Sockets;
using Moq;
using Xunit;

namespace Chorizo.Tests
{
    public class DotNetSocketMachineTest
    {
        private readonly int _testPort;
        private readonly string _testHostName;
        private readonly Mock<ICzoSocket> _mockCzoSocket;
        private readonly Mock<IConnectionBuilder> _mockConnectionBuilder;

        public DotNetSocketMachineTest()
        {
            _testPort = 8000;
            _testHostName = "localhost";
            _mockCzoSocket = new Mock<ICzoSocket>();
            _mockConnectionBuilder = new Mock<IConnectionBuilder>();
        }
        [Fact]
        public void ListenCreatesANewSocketAndStartsListeningOnIt()
        {
            var testSocketMachine = new DotNetSocketMachine
            {
                SocketImplementation = _mockCzoSocket.Object,
                ConnectionBuilder = _mockConnectionBuilder.Object
            };
            
            testSocketMachine.Listen(_testPort, _testHostName);
            
            _mockCzoSocket.Verify(sock => sock.Bind(It.IsAny<IPEndPoint>()));
            _mockCzoSocket.Verify(sock => sock.Listen(_testPort));
        }

        [Fact]
        public void AcceptConnection_AcceptConnectionsComingFromTheSocketAndReturnsAConnection()
        {
            var testSpecificConnectionBuilder = new Mock<IConnectionBuilder>();
            testSpecificConnectionBuilder.Setup(bob => bob.Build(It.IsAny<Socket>())).Returns(new Connection());
            var testSocketMachine = new DotNetSocketMachine
            {
                SocketImplementation = _mockCzoSocket.Object,
                ConnectionBuilder = testSpecificConnectionBuilder.Object
            };

            var result = testSocketMachine.AcceptConnection();
            
            _mockCzoSocket.Verify(sock => sock.Accept());
            testSpecificConnectionBuilder.Verify(bob => bob.Build(It.IsAny<Socket>()));
            Assert.IsType<Connection>(result);

        }
    }
}