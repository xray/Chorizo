using System;
using System.Net;
using System.Net.Sockets;

namespace Chorizo
{
    public interface ICzoSocket {
        Tuple<byte[], int> Receive(int byteBufferSize);
    }
}