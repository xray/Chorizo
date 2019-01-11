using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoChorizoShould
    {
        [Fact]
        public void Listen_ShouldStartListeningOnDefaultPortUsingSocketMachine()
        {
            // Arrange
            var mockRouter = new Mock<IRouter>();
            var mockSocketMachine = new Mock<ISocketMachine>();
            
            var localServer = new Chorizo
            {
                Router = mockRouter.Object,
                SocketMachine = mockSocketMachine.Object
            };
            
            // Act
            localServer.Listen();
            
            // Assert
            mockSocketMachine.Verify(sm => sm.Listen(8000));
        }
    }
}
