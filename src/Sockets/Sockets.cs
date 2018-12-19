using System;
using System.Net;
using System.Net.Sockets;

namespace ChorizoFW.Sockets
{
    public class SocketMaster
    {
        public static void AsyncCall(IAsyncResult result)
        {
            Socket FromEarlier = (Socket) result.AsyncState;
            Console.WriteLine("Something attempted to connect!");
        }

        public static void ListenMachine(Socket toListen, bool keepAlive)
        {
            if (keepAlive)
            {
                if (toListen.Poll(1000, SelectMode.SelectRead))
                {
                    toListen.BeginAccept(new AsyncCallback(AsyncCall), toListen);
                }
                else
                {
                    ListenMachine(toListen, true);
                }
            }
        }
        
        public static void NewSocket(int port)
        {
            // create the socket
            Socket listenSocket = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Stream,
                ProtocolType.Tcp
            );

            // bind the listening socket to the port
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            listenSocket.Bind(endPoint);

            // start listening
            listenSocket.Listen(Int32.MaxValue);
            ListenMachine(listenSocket, true);
        }
    }
}