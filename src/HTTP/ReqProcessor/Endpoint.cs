using System.Linq;

namespace Chorizo.HTTP.ReqProcessor
{
    public struct Endpoint
    {
        public readonly string Path;
        public readonly Route[] Routes;

        public Endpoint(string path, string method, Action action)
        {
            Path = path;
            Routes = new []{new Route(method, action)};
        }

        public Endpoint RegisterRoute(string method, Action action)
        {
            throw new System.NotImplementedException();
        }

        public bool SupportsMethod(string methodName)
        {
            return Routes.Any(method => method.Name == methodName);
        }
    }
}
