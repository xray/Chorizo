using System;

namespace Chorizo
{
    public interface IServerStatus
    {
        bool IsRunning();
    }
    public class Chorizo
    {
        public ISocketMachine SocketMachine { get; set; }
        public IRouter Router { get; set; }
        public IServerStatus Status { get; set; }
        private IConfigRetriever ConfigRetriever { get; set; }
        private readonly ServerConfig _config;
        public Chorizo()
        {
            _config = ConfigRetriever.GetConfig();
        }

        public void Start()
        {
            SocketMachine.Listen(_config.Port, _config.HostName);
            while (Status.IsRunning())
            {
                var chorizoSocket = SocketMachine.AcceptConnection();
            }
        }
    }
}