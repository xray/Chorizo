using Chorizo.Logger;
using Chorizo.ProtocolHandler.DataParser;

namespace Chorizo.ProtocolHandler.ResponseRetriever
{
    public class ResRetriever:IResponseRetriever
    {
        private readonly string _currentTime;

        public ResRetriever(IDateTimeProvider dateTimeProvider)
        {
            _currentTime = $"{dateTimeProvider.Now():R}";
        }

        public Response Retrieve(Request req)
        {
            if (req.Path() == "/simple_get")
            {
                return new Response("HTTP/1.1", 200, "OK")
                    .AddHeader("Date", _currentTime);
            }

            return new Response("HTTP/1.1", 404, "Not Found")
                .AddHeader("Date", _currentTime);
        }
    }
}
