using System;

namespace Chorizo.HTTP.ReqProcessor
{
    public class Routes
    {
        private readonly Endpoint[] _endpoints;
        public Routes()
        {
            _endpoints = new Endpoint[0];
        }

        private Routes(Endpoint[] endpoints)
        {
            _endpoints = endpoints;
        }

        public Routes Get(string path, Action action)
        {
            return AddRoute("GET", path, action);
        }

        public Route RetrieveRoute(string method, string path)
        {
            foreach (var route in _endpoints)
            {
                if (route.Path == path && route.SupportsMethod(method))
                {
                    return route;
                }
            }

            return null;
        }

        public bool HasMatchingRoute(string method, string path)
        {
            throw new NotImplementedException();
        }

        private Routes AddRoute(string method, string path, Action action)
        {
            if (HasMatchingRoute(method, path))
            {
                // replace with either a custom exception or one that indicates an attempted creation of a pre-existing route
                throw new NotImplementedException();
            }

            var newRoutes = new Endpoint[_endpoints.Length + 1];
            Array.Copy(_endpoints, newRoutes, _endpoints.Length);
            newRoutes[_endpoints.Length] = new Endpoint(method, path, action);

            return new Routes(newRoutes);
        }
    }
}
