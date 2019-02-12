using Chorizo.SocketMachine;
using Chorizo.Sockets.DotNetSocket;
using Moq;
using Xunit;

namespace Chorizo.Tests.SocketMachine
{
    public class DotNetSocketMachineTest
    {
        private readonly Mock<IDotNetSocket> _mockDotNetSocket;

        public DotNetSocketMachineTest()
        {
            _mockDotNetSocket = new Mock<IDotNetSocket>();
        }

        [Fact]
        public void SetupCallsBindOnTheBuiltInSocketWithPassedParams()
        {
            var testSm = new DotNetSocketMachine(_mockDotNetSocket.Object);

            testSm.Configure(25565, "localhost");

            _mockDotNetSocket.Verify(dotnet => dotnet.Bind(25565, "localhost"));
        }

        [Fact]
        public void ListenCallsListenOnTheBuiltInSocketWithPassedParams()
        {
            var testSm = new DotNetSocketMachine(_mockDotNetSocket.Object);

            testSm.Listen(100);

            _mockDotNetSocket.Verify(dotnet => dotnet.Listen(100));
        }

        [Fact]
        public void AcceptConnectionCallsAcceptOnTheBuiltInSocket()
        {
            var testSm = new DotNetSocketMachine(_mockDotNetSocket.Object);

            testSm.AcceptConnection();

            _mockDotNetSocket.Verify(dotnet => dotnet.Accept());
        }
    }
}
