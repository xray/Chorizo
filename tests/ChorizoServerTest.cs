using Chorizo.ProtocolHandler;
using Chorizo.Sockets.CzoSocket;
using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoServerTest
    {
        private readonly Mock<IChorizoSocket> _mockCzoSocket;
        private readonly Mock<IServerStatus> _mockServerStatus;
        private readonly Mock<ISocketMachine> _mockSocketMachine;
        private readonly Mock<IChorizoProtocolConnectionHandler> _mockHttpHandler;
        
        public ChorizoServerTest()
        {
            _mockCzoSocket = new Mock<IChorizoSocket>();
            _mockSocketMachine = new Mock<ISocketMachine>();
            _mockSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(_mockCzoSocket.Object);
            _mockServerStatus = new Mock<IServerStatus>();
            _mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(false);
            _mockHttpHandler = new Mock<IChorizoProtocolConnectionHandler>();
            _mockHttpHandler.Setup(http => http.WillHandle("HTTP")).Returns(true);
        }

        [Fact]
        public void Start_ShouldStartListeningOnDefaultPortAndHostNameUsingSocketMachine()
        {
            var localServer = new Chorizo
            {
                ProtocolConnectionHandler = _mockHttpHandler.Object,
                Status = _mockServerStatus.Object,
                SocketMachine = _mockSocketMachine.Object
            };
            
            localServer.Start();
            
            _mockSocketMachine.Verify(sm => sm.Setup(8000, "localhost"));
            _mockSocketMachine.Verify(sm => sm.Listen(100));
        }

        [Fact]
        public void GetsAConnectionFromTheSocketMachine()
        {
            var localServer = new Chorizo
            {
                ProtocolConnectionHandler = _mockHttpHandler.Object,
                SocketMachine = _mockSocketMachine.Object,
                Status = _mockServerStatus.Object
            };
            
            localServer.Start();
            
            _mockSocketMachine.Verify(sm => sm.AcceptConnection());
        }

        [Fact]
        public void HandlesConnection()
        {
            var localServer = new Chorizo
            {
                Status = _mockServerStatus.Object,
                SocketMachine = _mockSocketMachine.Object,
                ProtocolConnectionHandler = _mockHttpHandler.Object
            };
            
            localServer.Start();
            
            _mockHttpHandler.Verify(http => http.WillHandle("HTTP"));
            _mockHttpHandler.Verify(http => http.HandleRequest(_mockCzoSocket.Object));
        }

        [Fact]
        public void AcceptsMultipleConnections()
        {
            var mockSocketMachine = new Mock<ISocketMachine>();
            var mockServerStatus = new Mock<IServerStatus>();
            var testSockOne = new Mock<IChorizoSocket>();
            var testSockTwo = new Mock<IChorizoSocket>();

            mockSocketMachine.SetupSequence(sm => sm.AcceptConnection())
                .Returns(testSockOne.Object)
                .Returns(testSockTwo.Object);

            mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            var localServer = new Chorizo()
            {
                Status = mockServerStatus.Object,
                SocketMachine = mockSocketMachine.Object,
                ProtocolConnectionHandler = _mockHttpHandler.Object
            };
            
            localServer.Start();
            
            _mockHttpHandler.Verify(http => http.WillHandle("HTTP"));
            _mockHttpHandler.Verify(http => http.HandleRequest(testSockOne.Object));
            _mockHttpHandler.Verify(http => http.WillHandle("HTTP"));
            _mockHttpHandler.Verify(http => http.HandleRequest(testSockTwo.Object));
        }
    }
}
