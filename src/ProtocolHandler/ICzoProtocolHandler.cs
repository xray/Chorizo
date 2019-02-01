namespace Chorizo.ProtocolHandler
{
    public interface ICzoProtocolHandler
    {
        bool WillHandle(string protocol);
        void Handle(ICzoSocket chorizoSocket);
    }
}