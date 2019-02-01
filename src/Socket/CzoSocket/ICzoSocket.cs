using System;
using System.Net.Sockets;

namespace Chorizo
{
    public interface ICzoSocket {
        Tuple<byte[], int> Receive(int byteBufferSize);
        int Send(byte[] buffer);
        void Shutdown(SocketShutdown how);
        void Close();
    }
}