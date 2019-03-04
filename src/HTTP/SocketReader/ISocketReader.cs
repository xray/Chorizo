using Chorizo.Sockets.CzoSocket;

namespace Chorizo.HTTP.SocketReader
{
    public interface ISocketReader
    {
        byte[] ReadSocket(IChorizoSocket mockSocket);
    }
}
