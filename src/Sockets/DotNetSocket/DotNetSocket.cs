using System.Net;
using System.Net.Sockets;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.Sockets.DotNetSocket
{
    public class DotNetSocket : IDotNetSocket
    {
        private Socket _wrappedSocket;
        private bool _unbound;
        private bool _notListening;
        
        
        public DotNetSocket()
        {
            _unbound = true;
            _notListening = true;
        }
        
        
        public void Bind(int port, string hostName)
        {
            // Don't do this, just check to see if the _wrappedSocket is null or not
            // Same goes 
            if (_unbound != true)
            {
                throw new System.NotImplementedException();
            }
            
            var ipAddress = Dns.GetHostEntry(hostName).AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, port);
            _wrappedSocket = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            _wrappedSocket.Bind(localEndPoint);
            _unbound = false;
        }

        public void Listen(int backlogSize)
        {
            if (_unbound)
            {
                throw new System.NotImplementedException();
            }

            _wrappedSocket.Listen(backlogSize);
            _notListening = false;
        }

        public IChorizoSocket Accept()
        {
            if (_notListening)
            {
                throw new System.NotImplementedException();
            }
            return new ChorizoSocket(_wrappedSocket.Accept());
        }
    }
}