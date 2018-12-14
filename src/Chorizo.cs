using Chorizo.Sockets;
using System;

namespace Chorizo
{
    public class Server
    {
        public static bool Printer()
        {
            SocketMaster.Ident();
            SocketMaster.NewSocket();
            
            if (SocketMaster.No())
            {
                Console.WriteLine("How tho?");
            }
            else
            {
                Console.WriteLine("Just put Drake smiling here...");
            }
            return SocketMaster.No();
        }

        public static void Main(string[] args)
        {
            Server.Printer();
        }
    }
}
