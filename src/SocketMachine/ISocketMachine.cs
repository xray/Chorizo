using Chorizo.Sockets.InternalSocket;

namespace Chorizo
{
    public interface ISocketMachine
    {
        void Configure(int port, string hostName);
        void Listen(int backlog);
        IAppSocket AcceptConnection();
    }
}
