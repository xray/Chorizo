using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler
{
    public interface IChorizoProtocolHandler
    {
        bool WillHandle(string protocol);
        void Handle(IChorizoSocket chorizoSocket);
    }
}