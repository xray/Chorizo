using System;

namespace Chipotle
{
    class Program
    {
        public static void Main()
        {
            var port = 5000;
            var app = new Chorizo.Chorizo(port);

            app.ProtocolConnectionHandler.Router().Get("/", (req, res) =>
            {
                res.Status(200).Send("<html><h1>Hello World!</h1><h2>I am Chorizo</h2></html>");
            });

            app.ProtocolConnectionHandler.Router().Get("/simple_get", (req, res) =>
            {
                res.Status(200).Send();
            });

            Console.WriteLine($"Starting to listen on port {port}");
            app.Start();
        }
    }
}
