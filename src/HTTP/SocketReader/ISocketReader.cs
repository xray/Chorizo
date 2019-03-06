using Chorizo.HTTP.Exchange;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.HTTP.SocketReader
{
    public interface ISocketReader
    {
        byte[] ReadSocket(IChorizoSocket socket);
        Request ReadBody(IChorizoSocket socket, Request req);
    }
}
