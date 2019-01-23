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
                var (req, res) = SocketMachine.AcceptConnection();
                Router.Route(req, res);
                
            }
        }
    }
}