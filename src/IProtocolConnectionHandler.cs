using Chorizo.Sockets.CzoSocket;

namespace Chorizo
{
    public interface IProtocolConnectionHandler
    {
        void HandleRequest(IChorizoSocket chorizoSocket);
    }
}
