using System;
using System.Net;
using System.Text;

namespace Chorizo
{
    public class DotNetSocketMachine:ISocketMachine
    {
        public ICzoSocket SocketImplementation;
        public IRequestBuilder RequestBuilder;
        
        public void Listen(int port, string hostName)
        {
            var ipAddress = Dns.GetHostEntry(hostName).AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, port);
            
            SocketImplementation.Bind(localEndPoint);
            SocketImplementation.Listen(port);
        }

        public Tuple<Request, Response> AcceptConnection()
        {
            var workingSocket = SocketImplementation.Accept();
            var constructedRequest = RequestBuilder.Build("");
            return new Tuple<Request, Response>(constructedRequest, new Response());
        }

        public Tuple<string, byte[]> GetData()
        {
            var bufferText = "";
            var receivedHeader = new byte[0];
            
            while (bufferText.IndexOf("\r\n\r\n") == -1)
            {
                var (data, dataLength) = SocketImplementation.Receive(new byte[256]);
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