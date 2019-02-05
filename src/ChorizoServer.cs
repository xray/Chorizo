using Chorizo.ProtocolHandler;
using Chorizo.ServerConfiguration;
using Chorizo.SocketMachine;

namespace Chorizo
{
    public class Chorizo
    {
        private IServerStatus Status { get; }
        private ISocketMachine SocketMachine { get; }
        private IChorizoProtocolConnectionHandler ProtocolConnectionHandler { get; }
        private ServerConfig Config { get; }

        public Chorizo(
            int port = 8000,
            string protocol = "HTTP",
            IServerStatus serverStatus = null,
            ISocketMachine socketMachine = null,
            IChorizoProtocolConnectionHandler protocolConnectionHandler = null
        )
        {
            Config = new ServerConfig(protocol, "localhost", port);
            Status = serverStatus ?? new ServerStatus();
            SocketMachine = socketMachine ?? new DotNetSocketMachine();
            ProtocolConnectionHandler = protocolConnectionHandler ?? new ChorizoEchoConnectionHandler();
            SocketMachine.Configure(Config.Port, Config.HostName);
        }

        public void Start()
        {
            SocketMachine.Listen(Config.NumberOfConnectionsInQueue);
            while (Status.IsRunning())
            {
                var chorizoSocket = SocketMachine.AcceptConnection();
                ProtocolConnectionHandler.HandleRequest(chorizoSocket);
            }
        }
    }
}