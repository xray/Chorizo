using System;
using System.Net;
using System.Net.Sockets;

namespace Chorizo
{
    public interface ICzoSocket
    {
        void Bind(IPEndPoint endPoint);
        void Listen(int port);
        CzoSocket Accept();
        Tuple<byte[], int> Receive(byte[] buffer);
    }
}