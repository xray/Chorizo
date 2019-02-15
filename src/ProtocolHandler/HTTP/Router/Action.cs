using Chorizo.ProtocolHandler.HTTP.Requests;
using Chorizo.ProtocolHandler.HTTP.Responses;

namespace Chorizo.ProtocolHandler.HTTP.Router
{
    public delegate void Action(Request req, Response res);
}
