using System;

namespace Chorizo
{
    public interface ISocketMachine
    {
        void Listen(int port, string hostName, int backlog = 10);
        Tuple<Request, Response> AcceptConnection();
        Tuple<string, byte[]> GetHeader(ICzoSocket acceptedSocket);
    }
}