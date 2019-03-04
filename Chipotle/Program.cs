using System;

namespace Chipotle
{
    class Program
    {
        static void Main()
        {
            var port = 5000;
            var server = new Chorizo.Chorizo(port);
            Console.WriteLine($"Starting to listen on port {port}");
            server.Start();
        }
    }
}
