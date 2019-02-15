using Chorizo.ProtocolHandler.HTTP.Router;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler
{
    public interface IChorizoProtocolConnectionHandler
    {
        void HandleRequest(IChorizoSocket chorizoSocket);
        IRouter Router();
    }
}
