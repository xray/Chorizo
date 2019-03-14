using System.Net;
using System.Net.Sockets;
using Chorizo.Sockets.InternalSocket;

namespace Chorizo.Sockets.DotNetSocket
{
    public class DotNetSocket : IDotNetSocket
    {
        private Socket _wrappedSocket;
        
        public void Bind(int port, string hostName)
        {
            var ipAddress = Dns.GetHostEntry(hostName).AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, port);
            _wrappedSocket = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            _wrappedSocket?.Bind(localEndPoint);
        }

        public void Listen(int backlogSize)
        {
            _wrappedSocket?.Listen(backlogSize);
        }

        public IAppSocket Accept()
        {
            return new AppSocket(_wrappedSocket?.Accept());
        }
    }
}
