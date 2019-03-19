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
            var methods = _routes.GetAvailableMethods(req.Path);
            if (methods == "") return NotFoundResponse;

            if (req.Method == "OPTIONS")
            {
                if (methods != "")
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Server", "Chorizo")
                        .AddHeader("Allow", methods);
                }

                return NotFoundResponse;
            }

            if (req.Method != "HEAD")
                return _routes.RetrieveRoute(req.Method, req.Path)?.Handle(req) ?? new Response(
                               "HTTP/1.1",
                               405,
                               "Method Not Allowed"
                           )
                           .AddHeader("Server", "Chorizo")
                           .AddHeader("Allow", methods);

            var resWithBody = _routes.RetrieveRoute("GET", req.Path)?.Handle(req);
            if (resWithBody == null)
            {
                return _routes.RetrieveRoute(req.Method, req.Path)?.Handle(req) ?? NotFoundResponse;
            }

            return resWithBody.Value.SetBody("")
                .SetStatus(200, "OK");
        }
    }
}
