using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.DataParser
{
    public struct Request
    {
        public readonly string Method;
        public readonly string Path;
        public readonly string Protocol;
        public readonly Dictionary<string, string> Headers;

        public Request(string method, string path, string protocol, Dictionary<string, string> headers)
        {
            Method = method;
            Path = path;
            Protocol = protocol;
            Headers = headers;
        }
    }
}
