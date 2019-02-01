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
        public ICzoProtocolHandler ProtocolHandler;

        private ServerConfig Config { get; }

        public Chorizo()
        {
            ProtocolHandler = new CzoTelNetHandler();
            SocketMachine = SocketMachine ?? new DotNetSocketMachine();
            Config = new ServerConfig("HTTP", "localhost", 8000);
        }

        public Chorizo(int port, string protocol = null)
        {
            protocol = protocol ?? "HTTP";
            ProtocolHandler = new CzoTelNetHandler();
            SocketMachine = SocketMachine ?? new DotNetSocketMachine();
            Config = new ServerConfig(protocol, "localhost", port);
        }

        public void Start()
        {
            SocketMachine.Setup(Config.Port, Config.HostName);
            SocketMachine.Listen(Config.Backlog);
            while (Status.IsRunning())
            {
                var chorizoSocket = SocketMachine.AcceptConnection();
                if (ProtocolHandler.WillHandle(Config.Protocol))
                {
                    ProtocolHandler.Handle(chorizoSocket);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}