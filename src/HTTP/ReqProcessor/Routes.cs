using System.Collections.Generic;
using System.Linq;
using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.ReqProcessor
{
    public class Routes
    {
        private readonly List<Route> _routes;
        public Routes()
        {
            _routes = new List<Route>();
        }

        private Routes(List<Route> routes)
        {
            _routes = routes;
        }

        public Routes Get(string path, Action action)
        {
            return AddRoute("GET", path, action)
                .AddOptionsRoute(path);
        }

        public Routes Post(string path, Action action)
        {
            return AddRoute("POST", path, action)
                .AddOptionsRoute(path);
        }

        public Routes Put(string path, Action action)
        {
            return AddRoute("PUT", path, action)
                .AddOptionsRoute(path);
        }

        public Routes Head(string path, Action action)
        {
            return AddRoute("HEAD", path, action);
        }

        public Route? RetrieveRoute(string method, string path)
        {
            if (_routes.Exists(route => route.HttpMethod == method && route.Path == path))
            {
                return _routes.Find(route => route.HttpMethod == method && route.Path == path);
            }

            if (_routes.Exists(route => route.Path == path))
            {
                return new Route(method, path, req =>
                {
                    return new Response(
                            "HTTP/1.1",
                            405,
                            "Method Not Allowed"
                        )
                        .AddHeader("Server", "Chorizo")
                        .AddHeader("Allow", GetAvailableMethods(path));
                });
            }
            return new Route(method, path, req =>
            {
                return new Response("HTTP/1.1", 404, "Not Found")
                    .AddHeader("Server", "Chorizo");
            });
        }

        private Routes AddRoute(string method, string path, Action action)
        {
            var newRoutes = new List<Route>(_routes) {new Route(method, path, action)};
            return new Routes(newRoutes);
        }

        private Routes AddOptionsRoute(string path)
        {
            var adjustableRoutes = _routes.ToList();
            var optionsIndex = adjustableRoutes.FindIndex(route => route.Path == path && route.HttpMethod == "OPTIONS");
            if (optionsIndex != -1)
            {
                adjustableRoutes.RemoveAt(optionsIndex);
            }

            return new Routes(adjustableRoutes).AddRoute("OPTIONS", path, req =>
            {
                return new Response("HTTP/1.1", 200, "OK")
                    .AddHeader("Server", "Chorizo")
                    .AddHeader("Allow", GetAvailableMethods(path, adjustableRoutes));
            });
        }
        private string GetAvailableMethods(string reqPath, IEnumerable<Route> routes = null)
        {
            routes = routes ?? _routes;
            var options = new HashSet<string>();
            foreach (var route in routes)
            {
                if (route.Path == reqPath)
                {
                    if (route.HttpMethod == "GET") options.Add("HEAD");
                    options.Add(route.HttpMethod);
                }
            }
            options.Add("OPTIONS");
            return string.Join(",", options);
        }
    }
}
