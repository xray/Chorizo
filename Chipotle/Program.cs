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
            server.Start();
            Console.WriteLine($"Started listening on port {port}");

        }
    }
}