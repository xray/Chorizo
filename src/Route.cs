namespace Chorizo
{
    public class Route
    {
        public string HttpMethod { get; }
        public string RoutePath { get; }

        public delegate void Action(Request req, Response res);

        public readonly Action Go;
        public Route(string method, string path, Action action)
        {
            HttpMethod = method;
            RoutePath = path;
            Go = action;
        }
    }
}