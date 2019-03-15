using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.ReqProcessor
{
    public class NotARouter : IRequestProcessor
    {
        private readonly Routes _routes;
        private static readonly Response NotFoundResponse = new Response("HTTP/1.1", 404, "Not Found");

        public NotARouter(Routes routes)
        {
            _routes = routes;
        }
        public Response HandleRequest(Request req)
        {
            return _routes.RetrieveRoute(req.Method, req.Path)?.Handle(req) ?? NotFoundResponse;
        }
    }
}
