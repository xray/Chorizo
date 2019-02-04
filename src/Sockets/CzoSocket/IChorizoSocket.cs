using System;
using System.Net.Sockets;

namespace Chorizo.Sockets.CzoSocket
{
    public interface IChorizoSocket {
        Tuple<byte[], int> Receive(int byteBufferSize);
        int Send(byte[] buffer);
        void Disconnect(SocketShutdown how);
    }
}