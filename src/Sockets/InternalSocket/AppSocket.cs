using System;
using System.Net.Sockets;

namespace Chorizo.Sockets.InternalSocket
{
    public class AppSocket : IAppSocket
    {
        private readonly Socket _wrappedSocket;
        public AppSocket(Socket toWrap)
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

        public void Disconnect(SocketShutdown how)
        {
            _wrappedSocket.Shutdown(how);
            _wrappedSocket.Close();
        }
    }
}
