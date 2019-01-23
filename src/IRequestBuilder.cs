using System.Net.Sockets;

namespace Chorizo
{
    public interface IRequestBuilder
    {
        Request Build(string data);
    }
}