namespace Chorizo
{
    public interface IServerStatus
    {
        bool IsRunning();
    }
    public class Chorizo
    {
        public IServerStatus Status { get; set; }
        public ISocketMachine SocketMachine { get; set; }

        private readonly ServerConfig _config;
        
        public Chorizo()
        {
            _config = new ServerConfig("HTTP", "localhost", 8000);
        }

        public Chorizo(string protocol, int port)
        {
            _config = new ServerConfig(protocol, "localhost", port);
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