using System;
using System.Net.Sockets;
using System.Text;

namespace Chorizo.Sockets.CzoSocket
{
    public class ChorizoSocket : IChorizoSocket
    {
        private readonly Socket _wrappedSocket;
        public ChorizoSocket(Socket toWrap)
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

        public void Disconnect(SocketShutdown how = SocketShutdown.Both)
        {
            _wrappedSocket.Shutdown(how);
            _wrappedSocket.Close();
        }
    }
}
