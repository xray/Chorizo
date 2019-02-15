using System.Collections.Generic;
using Chorizo.ProtocolHandler.HTTP.Requests;
using Chorizo.ProtocolHandler.HTTP.Responses;

namespace Chorizo.ProtocolHandler.HTTP.Router
{
    public class ChorizoRouter:IRouter
    {
        private Dictionary<string, Route> _routes;
        public ChorizoRouter()
        {
            _routes = new Dictionary<string, Route>();
        }

        public void Match(Request req, Response res)
        {
            if (_routes.ContainsKey(req.Path) && _routes[req.Path].HttpMethod == req.Method)
            {
                _routes[req.Path].Go(req, res);
            }
            else
            {
                res.Status(404).Send("Not Found");
            }
        }

        public void Get(string path, Action action)
        {
            _routes.Add(path, new Route("GET", action));
        }
    }
}
