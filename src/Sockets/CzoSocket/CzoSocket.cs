using System;
using System.Net.Sockets;

namespace Chorizo.Sockets.CzoSocket
{
    public class CzoSocket : ICzoSocket
    {
        private readonly Socket _wrappedSocket;
        public CzoSocket(Socket toWrap)
        {
            _wrappedSocket = toWrap;
        }

        public Tuple<byte[], int> Receive(int byteBufferSize)
        {
            var receiveBuffer = new byte[byteBufferSize];
            var bytesReceived = _wrappedSocket.Receive(receiveBuffer);
            return new Tuple<byte[], int>(receiveBuffer, bytesReceived);
        }

        public int Send(byte[] buffer)
        {
            return _wrappedSocket.Send(buffer);
        }

        public void Shutdown(SocketShutdown how)
        {
            _wrappedSocket.Shutdown(how);
        }

        public void Close()
        {
            _wrappedSocket.Close();
        }
    }
}