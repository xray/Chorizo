namespace Chorizo
{
    public interface IRouter
    {
        bool Match(string requestMethod, string uri);
    }
}