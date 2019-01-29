using System.Net;

namespace Chorizo
{
    public interface IDotNetSocket
    {
        void Bind(IPEndPoint localEndPoint);
        void Listen(int backlog);
        ICzoSocket Accept();
    }
}