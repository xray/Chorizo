using System.Net.Sockets;

namespace Chorizo
{
    public interface IConnectionBuilder
    {
        Connection Build(Socket workingSocket);
    }
}