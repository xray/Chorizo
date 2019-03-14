using Chorizo.Sockets.InternalSocket;

namespace Chorizo.Sockets.DotNetSocket
{
    public interface IDotNetSocket
    {
        void Bind(int port, string hostName);
        void Listen(int backlogSize = 100);
        IAppSocket Accept();
    }
}
