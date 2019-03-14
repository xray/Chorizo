using System;
using System.Net.Sockets;

namespace Chorizo.Sockets.InternalSocket
{
    public interface IAppSocket {
        Tuple<byte[], int> Receive(int byteBufferSize);
        int Send(byte[] buffer);
        void Disconnect(SocketShutdown how);
    }
}
