using System.Net;

namespace Chorizo
{
    public interface IEndpointFetcher
    {
        IPEndPoint Fetch(int port);
    }
}