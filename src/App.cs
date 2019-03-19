using Chorizo.HTTP;
using Chorizo.HTTP.DataParser;
using Chorizo.HTTP.ReqProcessor;
using Chorizo.HTTP.SocketReader;
using Chorizo.Logger;
using Chorizo.Logger.Configuration;
using Chorizo.ServerConfiguration;
using Chorizo.SocketMachine;

namespace Chorizo
{
    public class App
    {
        private IServerStatus Status { get; }
        private ISocketMachine SocketMachine { get; }
        private IProtocolConnectionHandler ProtocolConnectionHandler { get; }
        private IMiniLogger Logger { get; }
        private ServerConfig Config { get; }


        public App(
            int port = Constants.Port,
            string mode = Constants.ServerMode,
            IServerStatus serverStatus = null,
            ISocketMachine socketMachine = null,
            IProtocolConnectionHandler protocolConnectionHandler = null,
            IMiniLogger logger = null,
            Routes routes = null
        )
        {
            Config = new ServerConfig(Constants.HostName, port, mode);
            Logger = logger ?? new MiniLogger(new LogConfig(Config.Mode));
            Status = serverStatus ?? new ServerStatus();
            SocketMachine = socketMachine ?? new DotNetSocketMachine();
            ProtocolConnectionHandler = protocolConnectionHandler ?? new HttpConnectionHandler
            {
                SocketReader = new InternalSocketReader(),
                DataParser = new RequestParser(),
                RequestProcessor = new RouteHandler(routes)

            };
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
