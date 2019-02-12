using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler
{
    public interface IChorizoProtocolConnectionHandler
    {
        void HandleRequest(IChorizoSocket chorizoSocket);
    }
}
