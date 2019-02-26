using System.Collections.Generic;
using Chorizo.Logger;
using Chorizo.ProtocolHandler.DataParser;

namespace Chorizo.ProtocolHandler.ResponseRetriever
{
    public class ResRetriever:IResponseRetriever
    {
        public IDateTimeProvider DateTimeProvider { get; set; }

        public Response Retrieve(Request req)
        {
            if (req.Path == "/simple_get")
            {
                return new Response("HTTP/1.1", 200, "OK")
                    .AddHeader("Date", $"{DateTimeProvider.Now():R}");
            }

            return new Response("HTTP/1.1", 404, "Not Found")
                .AddHeader("Date", $"{DateTimeProvider.Now():R}");
        }
    }
}
