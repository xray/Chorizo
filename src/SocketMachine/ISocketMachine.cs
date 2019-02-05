using Chorizo.Sockets.CzoSocket;

namespace Chorizo
{
    public interface ISocketMachine
    {
        void Configure(int port, string hostName);
        void Listen(int backlog);
        IChorizoSocket AcceptConnection();
    }
}