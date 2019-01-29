using System.Net;

namespace Chorizo
{
    public class DotNetSocket : IDotNetSocket
    {
        public void Bind(IPEndPoint localEndPoint)
        {
            throw new System.NotImplementedException();
        }

        public void Listen(int backlog)
        {
            throw new System.NotImplementedException();
        }

        public ICzoSocket Accept()
        {
            throw new System.NotImplementedException();
        }
    }
}