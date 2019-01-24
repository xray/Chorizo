using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chorizo
{
    public class DotNetSocketMachine:ISocketMachine
    {
        public ICzoSocket SocketImplementation;
        public IRequestBuilder RequestBuilder;

        public void Listen(int port, string hostName, int backlog = 0)
        {
            if (backlog == 0){ backlog = 10; }
            var ipAddress = Dns.GetHostEntry(hostName).AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, port);

            SocketImplementation = SocketImplementation ?? new CzoSocket(ipAddress.AddressFamily);
            
            SocketImplementation.Bind(localEndPoint);
            SocketImplementation.Listen(backlog);
        }

        public Tuple<Request, Response> AcceptConnection()
        {
            var workingSocket = SocketImplementation.Accept();
            var (requestHeader, extraBytes) = GetHeader(workingSocket);
            var constructedRequest = RequestBuilder.Build(requestHeader);
            return new Tuple<Request, Response>(constructedRequest, new Response());
        }

        public Tuple<string, byte[]> GetHeader(ICzoSocket acceptedSocket)
        {
            var bufferText = "";
            var receivedHeader = new byte[0];
            
            while (bufferText.IndexOf("\r\n\r\n") == -1)
            {
                var (data, dataLength) = acceptedSocket.Receive(256);
                var originalLength = receivedHeader.Length;
                Array.Resize(ref receivedHeader, originalLength + dataLength);
                Array.Copy(data, 0, receivedHeader, originalLength, dataLength);
                bufferText = Encoding.UTF8.GetString(receivedHeader, 0, receivedHeader.Length);
            }

            var endOfHeader = bufferText.IndexOf("\r\n\r\n") + 4;
            var onlyHeader = bufferText.Substring(0, endOfHeader);
            var headerLength = Encoding.UTF8.GetBytes(onlyHeader).Length;
            var extraData = new byte[receivedHeader.Length - headerLength];
            Buffer.BlockCopy(receivedHeader, headerLength, extraData, 0, extraData.Length);
            return new Tuple<string, byte[]>(onlyHeader, extraData);
        }
    }
}