using Chorizo.ProtocolHandler.DataParser;

namespace Chorizo.ProtocolHandler.ResponseRetriever
{
    public interface IResponseRetriever
    {
        Response Retrieve(Request req);
    }
}
