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
            return new Response(
                "HTTP/1.1",
                200,
                "OK",
                new Dictionary<string, string>
                {
                    {"Date", $"{DateTimeProvider.Now():R}"}
                }
            );
        }
    }
}
