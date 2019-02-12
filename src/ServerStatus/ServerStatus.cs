namespace Chorizo
{
    public class ServerStatus : IServerStatus
    {
        public bool IsRunning()
        {
            return true;
        }
    }
}
