namespace Chorizo.ServerConfiguration
{
    public class ServerConfig
    {
        public readonly string Protocol;
        public readonly string HostName;
        public readonly int Port;
        public readonly int NumberOfConnectionsInQueue;
        public readonly string Mode;

        public ServerConfig(string protocol, string hostName, int port, string mode, int numberOfConnectionsInQueue = 100)
        {
            Protocol = protocol;
            HostName = hostName;
            Port = port;
            NumberOfConnectionsInQueue = numberOfConnectionsInQueue;
            Mode = mode;
        }
    }
}