using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.ReqProcessor
{
    public interface IRequestProcessor
    {
        Response HandleRequest(Request req);
    }
}
