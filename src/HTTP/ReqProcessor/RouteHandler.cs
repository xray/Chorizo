using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.ReqProcessor
{
    public class RouteHandler : IRequestProcessor
    {
        private readonly Routes _routes;
        private static readonly Response NotFoundResponse = new Response("HTTP/1.1", 404, "Not Found");

        public RouteHandler(Routes routes)
        {
            _routes = routes;
        }

        public Response HandleRequest(Request req)
        {
            if (req.Method == "HEAD")
            {
                return _routes.RetrieveRoute("GET", req.Path).Value.Handle(req)
                    .SetBody("")
                    .SetStatus(200, "OK");
            }

            return _routes.RetrieveRoute(req.Method, req.Path).Value.Handle(req);
        }
    }
}
