using System;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chorizo.Sockets
{
    public class SocketMaster
    {
        static WaitHandle[] waitHandles = new WaitHandle[] 
        {
            new AutoResetEvent(false)
        };
        
        public static void Ident()
        {
            Console.WriteLine("This is Chorizo!");
        }

        public static bool No()
        {
            return false;
        }

        public static void AsyncBoi(IAsyncResult result)
        {
            Socket FromEarlier = (Socket) result.AsyncState;
            Console.WriteLine("Yo, got that connection B!");
        }

        public static void ListenMachine(Socket toListen, bool keepAlive)
        {
            if (keepAlive)
            {
                if (toListen.Poll(1000, SelectMode.SelectRead))
                {
                    toListen.BeginAccept(new AsyncCallback(AsyncBoi), toListen);
                }
                else
                {
                    ListenMachine(toListen, true);
                }
            }
        }
        
        public static void NewSocket()
        {
            // create the socket
            Socket listenSocket = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Stream,
                ProtocolType.Tcp
            );

            // bind the listening socket to the port
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 5000);
            listenSocket.Bind(endPoint);

            // start listening
            listenSocket.Listen(Int32.MaxValue);
            ListenMachine(listenSocket, true);
        }
    }
}