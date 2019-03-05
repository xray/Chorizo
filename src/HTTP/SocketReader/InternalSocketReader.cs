using System;
using System.Text;
using Chorizo.HTTP.Exchange;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.HTTP.SocketReader
{
    public class InternalSocketReader:ISocketReader
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

        public Request ReadBody(IChorizoSocket socket, Request req)
        {
            if (!req.ContainsHeader("Content-Length")) return req;
            var contentLength = int.Parse(req.GetHeader("Content-Length").Value());
            var (bytesRead, readByteCount) = socket.Receive(contentLength);
            var body = Encoding.UTF8.GetString(bytesRead);
            return new Request(req, body);

        }
    }
}
