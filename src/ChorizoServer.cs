using System.Threading;

namespace Chorizo
{
    public interface IServerStatus
    {
        bool IsRunning();
    }
    public class Chorizo
    {
        public ISocketMachine SocketMachine { get; set; }
        public IConnectionHandler ConnectionHandler { get; set; }
        public IServerStatus Status { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }

        public Chorizo()
        {
            Port = 8000;
            HostName = "localhost";
        }

        public void Start()
        {
            SocketMachine.Listen(Port, HostName);
            while (Status.IsRunning())
            {
                var connection = SocketMachine.AcceptConnection();
                ConnectionHandler.Handle(connection);
            }
        }
    }
}