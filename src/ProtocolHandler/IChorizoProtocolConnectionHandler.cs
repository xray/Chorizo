using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler
{
    public interface IChorizoProtocolConnectionHandler
    {
        bool WillHandle(string protocol);
        void HandleRequest(IChorizoSocket chorizoSocket);
    }
}