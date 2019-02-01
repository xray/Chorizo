using Chorizo;

namespace Chipoltle
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
            var server = new Chorizo.Chorizo(8000, "TelNet")
            {
                Status = new ServerRunner()
            };
            server.Start();
        }
    }
}