using System.Net;
using System.Net.Sockets;

namespace Chorizo
{
    public interface ICzoSocket
    {
        void Bind(IPEndPoint endPoint);
        void Listen(int port);
        Socket Accept();
    }
}