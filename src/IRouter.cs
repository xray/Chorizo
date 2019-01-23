namespace Chorizo
{
    public interface IRouter
    {
        void Route(Request req, Response res);
    }
}