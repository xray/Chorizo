using Chorizo.Sockets.InternalSocket;
using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class AppTest
    {
        private readonly Mock<IAppSocket> _mockCzoSocket;
        private readonly Mock<IServerStatus> _mockServerStatus;
        private readonly Mock<ISocketMachine> _mockSocketMachine;
        private readonly Mock<IProtocolConnectionHandler> _mockHttpHandler;

        public AppTest()
        {
            _mockCzoSocket = new Mock<IAppSocket>();
            _mockSocketMachine = new Mock<ISocketMachine>();
            _mockSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(_mockCzoSocket.Object);
            _mockServerStatus = new Mock<IServerStatus>();
            _mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(false);
            _mockHttpHandler = new Mock<IProtocolConnectionHandler>();
        }

        [Fact]
        public void Start_ShouldStartListeningOnDefaultPortAndHostNameUsingSocketMachine()
        {
            var localServer = new App(
                serverStatus: _mockServerStatus.Object,
                socketMachine: _mockSocketMachine.Object,
                protocolConnectionHandler:_mockHttpHandler.Object
            );

            localServer.Start();

            _mockSocketMachine.Verify(sm => sm.Configure(8000, "localhost"));
            _mockSocketMachine.Verify(sm => sm.Listen(100));
        }

        [Fact]
        public void GetsAConnectionFromTheSocketMachine()
        {
            var localServer = new App(
                serverStatus: _mockServerStatus.Object,
                socketMachine: _mockSocketMachine.Object,
                protocolConnectionHandler:_mockHttpHandler.Object
            );

            localServer.Start();

            _mockSocketMachine.Verify(sm => sm.AcceptConnection());
        }

        [Fact]
        public void HandlesConnection()
        {
            var localServer = new App(
                serverStatus: _mockServerStatus.Object,
                socketMachine: _mockSocketMachine.Object,
                protocolConnectionHandler:_mockHttpHandler.Object
            );

            localServer.Start();

            _mockHttpHandler.Verify(http => http.HandleRequest(_mockCzoSocket.Object));
        }

        [Fact]
        public void AcceptsMultipleConnections()
        {
            var mockSocketMachine = new Mock<ISocketMachine>();

            var mockServerStatus = new Mock<IServerStatus>();
            var testSockOne = new Mock<IAppSocket>();
            var testSockTwo = new Mock<IAppSocket>();

            mockSocketMachine.SetupSequence(sm => sm.AcceptConnection())
                .Returns(testSockOne.Object)
                .Returns(testSockTwo.Object);

            mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            var localServer = new App(
                serverStatus: mockServerStatus.Object,
                socketMachine: mockSocketMachine.Object,
                protocolConnectionHandler:_mockHttpHandler.Object
            );

            localServer.Start();

            _mockHttpHandler.Verify(http => http.HandleRequest(testSockOne.Object));
            _mockHttpHandler.Verify(http => http.HandleRequest(testSockTwo.Object));
        }
    }
}
