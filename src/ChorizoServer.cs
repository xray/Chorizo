using System;
using Chorizo.ProtocolHandler;
using Chorizo.ServerConfiguration;
using Chorizo.SocketMachine;

namespace Chorizo
{
    public class Chorizo
    {
        public IServerStatus Status { get; set; }
        public ISocketMachine SocketMachine;
        public IChorizoProtocolConnectionHandler ProtocolConnectionHandler;

        private ServerConfig Config { get; }

        public Chorizo(int port = 8000, string protocol = "HTTP")
        {
            ProtocolConnectionHandler = new ChorizoEchoConnectionHandler();
            SocketMachine = SocketMachine ?? new DotNetSocketMachine();
            Config = new ServerConfig(protocol, "localhost", port);
        }

        public void Start()
        {
            SocketMachine.Setup(Config.Port, Config.HostName);
            SocketMachine.Listen(Config.NumberOfConnectionsInQueue);
            while (Status.IsRunning())
            {
                var chorizoSocket = SocketMachine.AcceptConnection();
                if (ProtocolConnectionHandler.WillHandle(Config.Protocol))
                {
                    ProtocolConnectionHandler.HandleRequest(chorizoSocket);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}