using Chorizo.HTTP.Exchange;
using Chorizo.Sockets.InternalSocket;

namespace Chorizo.HTTP.SocketReader
{
    public interface ISocketReader
    {
        byte[] ReadSocket(IAppSocket socket);
        Request ReadBody(IAppSocket socket, Request req);
    }
}
