using System;
using ChorizoFW;

namespace Chipotle
{
    public class Server
    {
        public static void Main(string[] args)
        {
           Chorizo app = new Chorizo();
           app.Listen(5000);
        }
    }
}