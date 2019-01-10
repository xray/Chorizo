namespace Chorizo
{
    public interface IMatcher
    {
        Response Match(Request request);
    }
}
