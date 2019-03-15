using System;

namespace Chorizo.HTTP.ReqProcessor
{
    public class Routes
    {
        private readonly Route[] _routes;
        public Routes()
        {
            _routes = new Route[0];
        }

        private Routes(Route[] endpoints)
        {
            _routes = endpoints;
        }

        public Routes Get(string path, Action action)
        {
            return AddRoute("GET", path, action);
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
    }
}
