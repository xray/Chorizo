using Chorizo.ProtocolHandler.HTTP.Requests;
using Chorizo.ProtocolHandler.HTTP.Responses;

namespace Chorizo.ProtocolHandler.HTTP.Router
{
    public interface IRouter
    {
        void Match(IRequest req, IResponse res);

        void Get(string path, Action action);
    }
}
