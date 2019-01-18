using System.Threading;

namespace Chorizo
{
    public interface ISocketMachine
    {
        void Listen(int port, string hostName);
        Connection AcceptConnection();
    }
}