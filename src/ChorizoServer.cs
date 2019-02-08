using Chorizo.Logger;
using Chorizo.Logger.Configuration;
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
        private IMiniLogger Logger { get; }
        private ServerConfig Config { get; }
        private const string DevMode = "dev";
        private const int DefaultPort = 8000;

        public Chorizo(
            int port = DefaultPort,
            string mode = DevMode,
            IServerStatus serverStatus = null,
            ISocketMachine socketMachine = null,
            IChorizoProtocolConnectionHandler protocolConnectionHandler = null,
            IMiniLogger logger = null
        )
        {
            Config = new ServerConfig("localhost", port, mode);
            Logger = logger ?? new MiniLogger(new LogConfig(Config.Mode, "both"));
            Status = serverStatus ?? new ServerStatus();
            SocketMachine = socketMachine ?? new DotNetSocketMachine();
            ProtocolConnectionHandler = protocolConnectionHandler ?? new ChorizoEchoConnectionHandler(Logger);
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
