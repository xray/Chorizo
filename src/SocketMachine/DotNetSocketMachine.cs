using Chorizo.Sockets.CzoSocket;
using Chorizo.Sockets.DotNetSocket;

namespace Chorizo.SocketMachine
{
    public class DotNetSocketMachine : ISocketMachine
    {
        private IDotNetSocket BuiltInSocket { get; set; }

        public DotNetSocketMachine(IDotNetSocket builtInSocket = null)
        {
            BuiltInSocket = builtInSocket ?? new DotNetSocket();
        }

        public void Setup(int port, string hostName)
        {
            BuiltInSocket.Bind(port, hostName);
        }
        public void Listen(int backlog)
        {
            BuiltInSocket.Listen(backlog);
        }

        public IChorizoSocket AcceptConnection()
        {
            return BuiltInSocket.Accept();
        }
    }
}