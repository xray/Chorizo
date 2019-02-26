using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.ResponseRetriever
{
public struct Response
    {
        public readonly string Protocol;
        public readonly int StatusCode;
        public readonly string StatusText;
        public readonly Dictionary<string, string> Headers;

        public Response(string protocol, int statusCode, string statusText, Dictionary<string, string> headers)
        {
            Protocol = protocol;
            StatusCode = statusCode;
            StatusText = statusText;
            Headers = headers;
        }

        public bool Equals(Response other)
        {
             var requestLineValuesEqual =
                 Protocol == other.Protocol &&
                 StatusCode== other.StatusCode &&
                 StatusText == other.StatusText;
             if (!requestLineValuesEqual) return false;

             foreach (var (key, value) in Headers)
             {
                 if (!other.Headers.ContainsKey(key)) return false;
                 if (other.Headers[key] != value) return false;
             }

             foreach (var (key, value) in other.Headers)
             {
                 if (!Headers.ContainsKey(key)) return false;
                 if (Headers[key] != value) return false;
             }

             return true;
        }
    }
}
