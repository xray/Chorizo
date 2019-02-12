using System;

namespace Chipotle
{
    class Program
    {
        static void Main()
        {
            var port = 8000;
            var server = new Chorizo.Chorizo(port, "TelNet");
            Console.WriteLine($"Starting to listen on port {port}");
            server.Start();
        }
    }
}
