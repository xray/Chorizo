namespace Chorizo
{
    public class Chorizo
    {
        public IRouter Router { get; set; }
        public ISocketMachine SocketMachine { get; set; }
        public int Port { get; set; }

        public Chorizo()
        {
            Port = 8000;
        }

        public void Listen()
        {
            SocketMachine.Listen(Port);
        }
    }
}