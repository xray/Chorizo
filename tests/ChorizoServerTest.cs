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
        private readonly Mock<IChorizoProtocolHandler> _mockHTTPHandler;
        
        public ChorizoServerTest()
        {
            _mockCzoSocket = new Mock<IChorizoSocket>();
            _mockSocketMachine = new Mock<ISocketMachine>();
            _mockSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(_mockCzoSocket.Object);
            _mockServerStatus = new Mock<IServerStatus>();
            _mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(false);
            _mockHTTPHandler = new Mock<IChorizoProtocolHandler>();
            _mockHTTPHandler.Setup(http => http.WillHandle("HTTP")).Returns(true);
        }

        [Fact]
        public void Start_ShouldStartListeningOnDefaultPortAndHostNameUsingSocketMachine()
        {
            // Arrange
            var localServer = new Chorizo
            {
                ProtocolHandler = _mockHTTPHandler.Object,
                Status = _mockServerStatus.Object,
                SocketMachine = _mockSocketMachine.Object
            };
            
            // Act
            localServer.Start();
            
            // Assert
            _mockSocketMachine.Verify(sm => sm.Setup(8000, "localhost"));
            _mockSocketMachine.Verify(sm => sm.Listen(100));
        }

        [Fact]
        public void GetsAConnectionFromTheSocketMachine()
        {
            var localServer = new Chorizo
            {
                ProtocolHandler = _mockHTTPHandler.Object,
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
                ProtocolHandler = _mockHTTPHandler.Object
            };
            
            localServer.Start();
            
            _mockHTTPHandler.Verify(http => http.WillHandle("HTTP"));
            _mockHTTPHandler.Verify(http => http.Handle(_mockCzoSocket.Object));
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
                ProtocolHandler = _mockHTTPHandler.Object
            };
            
            localServer.Start();
            
            _mockHTTPHandler.Verify(http => http.WillHandle("HTTP"));
            _mockHTTPHandler.Verify(http => http.Handle(testSockOne.Object));
            _mockHTTPHandler.Verify(http => http.WillHandle("HTTP"));
            _mockHTTPHandler.Verify(http => http.Handle(testSockTwo.Object));
        }
    }
}
