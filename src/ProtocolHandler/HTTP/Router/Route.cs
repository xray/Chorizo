using Chorizo.ProtocolHandler.HTTP.Requests;
using Chorizo.ProtocolHandler.HTTP.Responses;

namespace Chorizo.ProtocolHandler.HTTP.Router
{
    public class Route
    {
        private readonly string _method;
        private readonly Action _action;
        public Route(string method, Action action)
        {
            _method = method;
            _action = action;
        }

        public void Go(IRequest req, IResponse res)
        {
            _action(req, res);
        }

        public string HttpMethod()
        {
            return _method;
        }
    }
}
