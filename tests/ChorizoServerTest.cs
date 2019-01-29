//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Threading;
//using Xunit;
//using Moq;
//
//namespace Chorizo.Tests
//{
//    public class ChorizoServerTest
//    {
//        private readonly Mock<IRouter> _mockRouter;
//        private readonly Mock<IServerStatus> _mockServerStatus;
//        private readonly Mock<ISocketMachine> _mockSocketMachine;
//        private readonly Request _testRequest;
//
//        public ChorizoServerTest()
//        {
//            _mockSocketMachine = new Mock<ISocketMachine>();
//            var testParams = new Dictionary<string, string>
//            {
//                {"cache-control", "no-cache"},
//                {"Postman-Token", "3f77f85c-b78e-46ef-94cc-a82b1cbacc86"},
//                {"User-Agent", "PostmanRuntime/7.4.0"},
//                {"Accept", "*/*"},
//                {"Host", "localhost:8000"},
//                {"accept-encoding", "gzip, deflate"},
//                {"Connection", "keep-alive"}
//            };
//            _testRequest = new Request("GET", "", "HTTP/1.1", testParams);
//            _mockSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(new CzoSocket());
//            _mockServerStatus = new Mock<IServerStatus>();
//            _mockServerStatus.SetupSequence(status => status.IsRunning())
//                .Returns(true)
//                .Returns(false);
//        }
//
//        [Fact]
//        public void Start_ShouldStartListeningOnDefaultPortAndHostNameUsingSocketMachine()
//        {
//            // Arrange
//            var localServer = new Chorizo
//            {
//                SocketMachine = _mockSocketMachine.Object,
//                Router = _mockRouter.Object,
//                Status = _mockServerStatus.Object,
//            };
//            
//            // Act
//            localServer.Start();
//            
//            // Assert
//            _mockSocketMachine.Verify(sm => sm.Listen(8000, "localhost", 10));
//        }
//
//        [Fact]
//        public void GetsAConnectionFromTheSocketMachine()
//        {
//            var localServer = new Chorizo
//            {
//                SocketMachine = _mockSocketMachine.Object,
//                Router = _mockRouter.Object,
//                Status = _mockServerStatus.Object
//            };
//            
//            localServer.Start();
//            
//            _mockSocketMachine.Verify(sm => sm.AcceptConnection());
//        }
//
//        [Fact]
//        public void HandlesConnection()
//        {
//            var mockRouter = new Mock<IRouter>();
//            var testSpecificSocketMachine = new Mock<ISocketMachine>();
//            var testReqRes = new CzoSocket();
//
//            testSpecificSocketMachine.Setup(sm => sm.AcceptConnection()).Returns(testReqRes);
//            
//            var localServer = new Chorizo
//            {
//                SocketMachine = testSpecificSocketMachine.Object,
//                Router = mockRouter.Object,
//                Status = _mockServerStatus.Object
//            };
//            
//            localServer.Start();
//            
//            mockRouter.Verify(ch => ch.Route(testReq, testRes));
//        }
//
//        [Fact]
//        public void AcceptsMultipleConnections()
//        {
//            var mockSocketMachine = new Mock<ISocketMachine>();
//            var mockRouter = new Mock<IRouter>();
//            var mockServerStatus = new Mock<IServerStatus>();
//            var reqRes1 = new CzoSocket();
//            var reqRes2 = new CzoSocket();
//
//            mockSocketMachine.SetupSequence(sm => sm.AcceptConnection())
//                .Returns(reqRes1)
//                .Returns(reqRes2);
//
//            mockServerStatus.SetupSequence(status => status.IsRunning())
//                .Returns(true)
//                .Returns(true)
//                .Returns(false);
//
//            var localServer = new Chorizo()
//            {
//                SocketMachine = _mockSocketMachine.Object,
//                Router = mockRouter.Object,
//                Status = mockServerStatus.Object
//            };
//            
//            localServer.Start();
//            
//            mockRouter.Verify(router => router.Route(mockReq1, mockRes1));
//            mockRouter.Verify(router => router.Route(mockReq2, mockRes2));
//        }
//    }
//}
