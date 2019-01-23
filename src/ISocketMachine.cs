using System;

namespace Chorizo
{
    public interface ISocketMachine
    {
        void Listen(int port, string hostName);
        Tuple<Request, Response> AcceptConnection();
        Tuple<string, byte[]> GetData();
    }
}