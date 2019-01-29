using System;

namespace Chorizo
{
    public interface ISocketMachine
    {
        void Listen(int port, string hostName, int backlog = 10);
        ICzoSocket AcceptConnection();
    }
}