using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoChorizoShould
    {
        [Fact]
        public void Listen_ShouldStartListeningOnDefaultPortUsingSocketMachine()
        {
            var mockRouter = new Mock<IRouter>();
            var mockSocketMachine = new Mock<ISocketMachine>();
            
            var localServer = new Chorizo()
            {
                Router = mockRouter.Object,
                SocketMachine = mockSocketMachine.Object
            };
            
            localServer.Listen();
            
            mockSocketMachine.Verify(sm => sm.Listen(8000));
        }
    }
}
