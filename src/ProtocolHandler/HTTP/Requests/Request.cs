using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public class Request
    {
        public readonly string Method;
        public readonly string Path;
        public readonly string Protocol;
        public readonly Dictionary<string, string> Headers;
        public readonly byte[] Body;

        public Request(ParsedRequestData reqData, byte[] body)
        {
            Method = reqData.Method;
            Path = reqData.Path;
            Protocol = reqData.Protocol;
            Headers = SanitizeHeaders(reqData.Headers);
            Body = body;
        }

        private static Dictionary<string, string> SanitizeHeaders(Dictionary<string, string> headers)
        {
            var sanitizedHeaders = new Dictionary<string, string>();

            foreach (var (key, value) in headers)
            {
                sanitizedHeaders.Add(key.ToUpper(), value);
            }

            return sanitizedHeaders;
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

            return Body == other.Body;
        }
    }
}
