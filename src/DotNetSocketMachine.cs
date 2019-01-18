using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chorizo
{
    public class DotNetSocketMachine:ISocketMachine
    {
        public ICzoSocket SocketImplementation;
        public IConnectionBuilder ConnectionBuilder;
        
        public void Listen(int port, string hostName)
        {
            var ipAddress = Dns.GetHostEntry(hostName).AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, port);
            
            SocketImplementation.Bind(localEndPoint);
            SocketImplementation.Listen(port);
        }

        public Connection AcceptConnection()
        {
            var data = SocketImplementation.Accept();
            return ConnectionBuilder.Build(data);
        }
    }
}