using System;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo
{
    public interface ISocketMachine
    {
        void Setup(int port, string hostName);
        void Listen(int backlog);
        ICzoSocket AcceptConnection();
    }
}