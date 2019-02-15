using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public struct ParsedRequestData
    {
        public string Method;
        public string Path;
        public string Protocol;
        public Dictionary<string, string> Headers;
        public byte[] Body;
    }
}
