namespace Chorizo.ProtocolHandler.HTTP.Router
{
    public class Route
    {
        public string HttpMethod;

        public readonly Action Go;
        public Route(string method, Action action)
        {
            HttpMethod = method;
            Go = action;
        }
    }
}
