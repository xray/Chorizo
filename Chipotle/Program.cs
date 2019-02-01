using System;
using Chorizo;

namespace Chipotle
{
    public class ServerRunner : IServerStatus
    {
        public bool IsRunning()
        {
            return true;
        }
    }

    class Program
    {
        static void Main()
        {
            var port = 8000;
            var server = new Chorizo.Chorizo(port, "TelNet")
            {
                Status = new ServerRunner()
            };
            Console.WriteLine($"Starting to listen on port {port}");
            server.Start();
        }
    }
}