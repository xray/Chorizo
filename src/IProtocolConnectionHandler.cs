using Chorizo.Sockets.InternalSocket;

namespace Chorizo
{
    public interface IProtocolConnectionHandler
    {
        void HandleRequest(IAppSocket appSocket);
    }
}
