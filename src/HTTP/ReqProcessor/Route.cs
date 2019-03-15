using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.ReqProcessor
{
    public readonly struct Route
    {
        public readonly string HttpMethod;
        public readonly string Path;
        public readonly Action Action;

        public Route(string httpMethod, string name, Action action)
        {
            HttpMethod = httpMethod;
            Path = name;
            Action = action;
        }

        public Response Handle(Request request)
        {
            return Action.Invoke(request);
        }
    }
}
