using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public struct ParsedRequestData
    {
        public string Method;
        public string Path;
        public string Protocol;
        public Dictionary<string, string> Headers;

        public bool Equals(ParsedRequestData other)
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
