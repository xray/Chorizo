using System;
using System.Text;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.HTTP.SocketReader
{
    public class SocketReader:ISocketReader
    {
        public byte[] ReadSocket(IChorizoSocket socket)
        {
            var bytesToReturn = new byte[0];
            while (!Encoding.UTF8.GetString(bytesToReturn).Contains("\r\n\r\n"))
            {
                var (bytesRead, readByteCount) = socket.Receive(1);
                Array.Resize(ref bytesToReturn, bytesToReturn.Length + readByteCount);
                bytesToReturn[bytesToReturn.Length - readByteCount] = bytesRead[0];
            }

            return bytesToReturn;
        }
    }
}
