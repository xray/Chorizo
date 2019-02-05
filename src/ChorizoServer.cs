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

        public Chorizo(int port = 8000, string protocol = "HTTP"){
            Config = new ServerConfig(protocol, "localhost", port);
            Status = new ServerStatus();
            SocketMachine = new DotNetSocketMachine();
            ProtocolConnectionHandler = new ChorizoEchoConnectionHandler();
            SocketMachine.Configure(Config.Port, Config.HostName);
        }

        public Chorizo(
            int port,
            string protocol,
            IServerStatus serverStatus,
            ISocketMachine socketMachine,
            IChorizoProtocolConnectionHandler protocolConnectionHandler
        )
        {
            Config = new ServerConfig(protocol, "localhost", port);
            Status = serverStatus;
            SocketMachine = socketMachine;
            ProtocolConnectionHandler = protocolConnectionHandler;
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