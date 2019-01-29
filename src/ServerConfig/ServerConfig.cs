namespace Chorizo
{
    public class ServerConfig
    {
        public readonly string Protocol;
        public readonly string HostName;
        public readonly int Port;

        public ServerConfig(string protocol, string hostName, int port)
        {
            Protocol = protocol;
            HostName = hostName;
            Port = port;
        }
    }
}