namespace Chorizo
{
    public interface IDotNetSocket
    {
        void Bind(int port, string hostName);
        void Listen(int backlogSize = 100);
        ICzoSocket Accept();
    }
}