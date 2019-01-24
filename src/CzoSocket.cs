using System;
using System.Net;
using System.Net.Sockets;

namespace Chorizo
{
    public class CzoSocket : ICzoSocket
    {
        private Socket WrappedSocket { get; set; }
        public CzoSocket(AddressFamily addressFamily = AddressFamily.Unspecified)
        {
            if (addressFamily != AddressFamily.Unspecified)
            {
                WrappedSocket = new Socket(
                    addressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );
            }
        }
        public void Bind(IPEndPoint endPoint)
        {
            WrappedSocket.Bind(endPoint);
        }

        public void Listen(int backlogSize)
        {
            WrappedSocket.Listen(backlogSize);
        }

        public ICzoSocket Accept()
        {
            var returnSocket = WrappedSocket.Accept();
            return new CzoSocket
            {
                WrappedSocket = returnSocket
            };
        }

        public Tuple<byte[], int> Receive(int byteBufferSize)
        {
            var receiveBuffer = new byte[byteBufferSize];
            var bytesReceived = WrappedSocket.Receive(receiveBuffer);
            return new Tuple<byte[], int>(receiveBuffer, bytesReceived);
        }
    }
}