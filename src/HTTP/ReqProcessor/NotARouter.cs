using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.ReqProcessor
{
    public class NotARouter : IRequestProcessor
    {
        private readonly Routes _routes;
        public NotARouter(Routes routes)
        {
            _routes = routes;
        }
        public Response Process(Request req)
        {
            return new Response("HTTP/1.1", 404, "NOT FOUND");
        }
    }
}
