using Chorizo.Sockets.DotNetSocket;
using Chorizo.Sockets.InternalSocket;

namespace Chorizo.SocketMachine
{
    public class DotNetSocketMachine : ISocketMachine
    {
        private IDotNetSocket BuiltInSocket { get; }

        public DotNetSocketMachine(IDotNetSocket builtInSocket = null)
        {
            BuiltInSocket = builtInSocket ?? new DotNetSocket();
        }

        public void Configure(int port, string hostName)
        {
            BuiltInSocket.Bind(port, hostName);
        }
        public void Listen(int backlog)
        {
            BuiltInSocket.Listen(backlog);
        }

        public IAppSocket AcceptConnection()
        {
            return BuiltInSocket.Accept();
        }
    }
}
