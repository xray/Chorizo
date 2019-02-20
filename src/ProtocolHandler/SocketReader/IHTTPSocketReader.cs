using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler.SocketReader
{
    public interface IHTTPSocketReader
    {
        byte[] ReadSocket(IChorizoSocket mockSocket);
    }
}
