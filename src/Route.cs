namespace Chorizo
{
    public class Route
    {
        public string HttpMethod { get; }
        public string RoutePath { get; }

        public readonly DefaultRouter.Action Go;
        public Route(string method, string path, DefaultRouter.Action action)
        {
            HttpMethod = method;
            RoutePath = path;
            Go = action;
        }
    }
}