using System;
using System.Runtime.CompilerServices;
using ChorizoFW.Sockets;

namespace ChorizoFW
{
    public class Chorizo
    {
        public Chorizo Listen(int port)
        {
            SocketMaster.NewSocket(port);
            return this;
        }
    }
}
