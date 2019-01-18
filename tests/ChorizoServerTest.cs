using System.Collections.Generic;
using System.Data;
using System.Threading;
using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoServerTest
    {
        private readonly Mock<IConnectionHandler> _mockConnectionHandler;
        private readonly Mock<IServerStatus> _mockServerStatus;
        private readonly Mock<ISocketMachine> _mockSocketMachine;

        public ChorizoServerTest()
        {
            _mockSocketMachine = new Mock<ISocketMachine>();
            _mockConnectionHandler = new Mock<IConnectionHandler>();
            _mockServerStatus = new Mock<IServerStatus>();
            _mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(false);
        }

        [Fact]
        public void Start_ShouldStartListeningOnDefaultPortandHostNameUsingSocketMachine()
        {
            // Arrange
            var localServer = new Chorizo
            {
                SocketMachine = _mockSocketMachine.Object,
                ConnectionHandler = _mockConnectionHandler.Object,
                Status = _mockServerStatus.Object,
            };
            
            // Act
            localServer.Start();
            
            // Assert
            _mockSocketMachine.Verify(sm => sm.Listen(8000, "localhost"));
        }

        [Fact]
        public void GetsAConnectionFromTheSocketMachine()
        {
            var localServer = new Chorizo
            {
                SocketMachine = _mockSocketMachine.Object,
                ConnectionHandler = _mockConnectionHandler.Object,
                Status = _mockServerStatus.Object
            };
            
            localServer.Start();
            
            _mockSocketMachine.Verify(sm => sm.AcceptConnection());
        }

        [Fact]
        public void HandlesConnection()
        {
            var mockConnectionHandler = new Mock<IConnectionHandler>();
            var testSpecificSocketMachine = new Mock<ISocketMachine>();
            var testConnection = new Connection();

            testSpecificSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(testConnection);
            
            var localServer = new Chorizo()
            {
                SocketMachine = testSpecificSocketMachine.Object,
                ConnectionHandler = mockConnectionHandler.Object,
                Status = _mockServerStatus.Object
            };
            
            localServer.Start();
            
            mockConnectionHandler.Verify(ch => ch.Handle(testConnection));
        }

        [Fact]
        public void AcceptsMultipleConnections()
        {
            var mockSocketMachine = new Mock<ISocketMachine>();
            var mockConnectionHandler = new Mock<IConnectionHandler>();
            var mockServerStatus = new Mock<IServerStatus>();
            var connection1 = new Connection();
            var connection2 = new Connection();

            mockSocketMachine.SetupSequence(sm => sm.AcceptConnection())
                .Returns(connection1)
                .Returns(connection2);

            mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            var localServer = new Chorizo()
            {
                SocketMachine = _mockSocketMachine.Object,
                ConnectionHandler = mockConnectionHandler.Object,
                Status = mockServerStatus.Object
            };
            
            localServer.Start();
            
            mockConnectionHandler.Verify(ch => ch.Handle(It.IsAny<Connection>()));
            mockConnectionHandler.Verify(ch => ch.Handle(It.IsAny<Connection>()));
        }
    }
}
