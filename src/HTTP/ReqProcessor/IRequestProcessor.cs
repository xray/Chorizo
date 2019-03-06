using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.ReqProcessor
{
    public interface IRequestProcessor
    {
        Response Process(Request req);
    }
}
