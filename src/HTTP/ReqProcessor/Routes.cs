using System;
using System.Collections.Generic;

namespace Chorizo.HTTP.ReqProcessor
{
    public class Routes
    {
        private readonly Route[] _routes;
        public Routes()
        {
            _routes = new Route[0];
        }

        private Routes(Route[] routes)
        {
            _routes = routes;
        }

        public Routes Get(string path, Action action)
        {
            return AddRoute("GET", path, action);
        }

        public Routes Post(string path, Action action)
        {
            return AddRoute("POST", path, action);
        }

        public Routes Put(string path, Action action)
        {
            return AddRoute("PUT", path, action);
        }

        public Routes Head(string path, Action action)
        {
            return AddRoute("HEAD", path, action);
        }

        public Route? RetrieveRoute(string method, string path)
        {
            foreach (var route in _routes)
                if (route.Path == path && route.HttpMethod == method)
                    return route;

            return null;
        }

        private Routes AddRoute(string method, string path, Action action)
        {
            var newRoutes = new Route[_routes.Length + 1];
            Array.Copy(_routes, newRoutes, _routes.Length);
            newRoutes[_routes.Length] = new Route(method, path, action);

            return new Routes(newRoutes);
        }

        public string GetAvailableMethods(string reqPath)
        {
            var options = new List<string>();
            foreach (var route in _routes)
            {
                if (route.Path == reqPath)
                {
                    options.Add(route.HttpMethod);
                    if(route.HttpMethod == "GET") options.Add("HEAD");
                }
            }
            if(options.Count != 0) options.Add("OPTIONS");

            return string.Join(",", options);
        }
    }
}
