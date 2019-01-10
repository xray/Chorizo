namespace Chorizo
{
    public interface IRouter
    {
        Response Match(Request request);
    }
}