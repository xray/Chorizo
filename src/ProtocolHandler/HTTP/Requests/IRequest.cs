using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public interface IRequest
    {
        string Method();
        string Path();
        string Protocol();
        Dictionary<string, string> Headers();
        byte[] Body();
    }
}
