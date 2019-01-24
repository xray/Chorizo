using System;
using System.Net;
using System.Net.Sockets;

namespace Chorizo
{
    public interface ICzoSocket
    {
        void Bind(IPEndPoint endPoint);
        void Listen(int backlogSize);
        ICzoSocket Accept();
        Tuple<byte[], int> Receive(int byteBufferSize);
    }
}