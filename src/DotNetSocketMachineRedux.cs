namespace Chorizo
{
    public class DotNetSocketMachineRedux : ISocketMachine
    {
        private IDotNetSocket BuiltInSocket;
        
        public DotNetSocketMachineRedux()
        {
            
        }
        public void Listen(int port, string hostName, int backlog = 10)
        {
            throw new System.NotImplementedException();
        }

        public ICzoSocket AcceptConnection()
        {
            throw new System.NotImplementedException();
        }
    }
}