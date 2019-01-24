using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoServerTest
    {
        private readonly Mock<IRouter> _mockRouter;
        private readonly Mock<IServerStatus> _mockServerStatus;
        private readonly Mock<ISocketMachine> _mockSocketMachine;

        public ChorizoServerTest()
        {
            _mockSocketMachine = new Mock<ISocketMachine>();
            _mockSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(new Tuple<Request, Response>(new Request(), new Response()));
            _mockRouter = new Mock<IRouter>();
            _mockServerStatus = new Mock<IServerStatus>();
            _mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(false);
        }

        [Fact]
        public void Start_ShouldStartListeningOnDefaultPortAndHostNameUsingSocketMachine()
        {
            // Arrange
            var localServer = new Chorizo
            {
                SocketMachine = _mockSocketMachine.Object,
                Router = _mockRouter.Object,
                Status = _mockServerStatus.Object,
            };
            
            // Act
            localServer.Start();
            
            // Assert
            _mockSocketMachine.Verify(sm => sm.Listen(8000, "localhost", 10));
        }

        [Fact]
        public void GetsAConnectionFromTheSocketMachine()
        {
            var localServer = new Chorizo
            {
                SocketMachine = _mockSocketMachine.Object,
                Router = _mockRouter.Object,
                Status = _mockServerStatus.Object
            };
            
            localServer.Start();
            
            _mockSocketMachine.Verify(sm => sm.AcceptConnection());
        }

        [Fact]
        public void HandlesConnection()
        {
            var mockRouter = new Mock<IRouter>();
            var testSpecificSocketMachine = new Mock<ISocketMachine>();
            var testReqRes = new Tuple<Request, Response>(new Request(), new Response());
            var (testReq, testRes) = testReqRes;

            testSpecificSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(testReqRes);
            
            var localServer = new Chorizo
            {
                SocketMachine = testSpecificSocketMachine.Object,
                Router = mockRouter.Object,
                Status = _mockServerStatus.Object
            };
            
            localServer.Start();
            
            mockRouter.Verify(ch => ch.Route(testReq, testRes));
        }

        [Fact]
        public void AcceptsMultipleConnections()
        {
            var mockSocketMachine = new Mock<ISocketMachine>();
            var mockRouter = new Mock<IRouter>();
            var mockServerStatus = new Mock<IServerStatus>();
            var reqRes1 = new Tuple<Request, Response>(new Request(), new Response());
            var reqRes2 = new Tuple<Request, Response>(new Request(), new Response());
            var (mockReq1, mockRes1) = reqRes1;
            var (mockReq2, mockRes2) = reqRes2;

            mockSocketMachine.SetupSequence(sm => sm.AcceptConnection())
                .Returns(reqRes1)
                .Returns(reqRes2);

            mockServerStatus.SetupSequence(status => status.IsRunning())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            var localServer = new Chorizo()
            {
                SocketMachine = _mockSocketMachine.Object,
                Router = mockRouter.Object,
                Status = mockServerStatus.Object
            };
            
            localServer.Start();
            
            mockRouter.Verify(router => router.Route(mockReq1, mockRes1));
            mockRouter.Verify(router => router.Route(mockReq2, mockRes2));
        }
    }
}
