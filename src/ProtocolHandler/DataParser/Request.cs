using System;
using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.DataParser
{
    public struct Request:IEquatable<Request>
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

        public bool Equals(Request other)
        {
            var mppMatch = Method == other.Method &&
                           Path == other.Path &&
                           Protocol == other.Protocol;

            if (!mppMatch) return false;

            foreach (var (key, value) in Headers)
            {
                if (!other.Headers.ContainsKey(key)) return false;
                if (other.Headers[key] != value) return false;
            }

            return true;
        }
    }
}
