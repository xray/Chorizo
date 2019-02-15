using System;
using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public class Request
    {
        public string Method;
        public string Path;
        public string Version;
        public Dictionary<string, string> Headers;
        public byte[] Body;

        public Request(ParsedRequestData reqData, byte[] body)
        {
            Method = reqData.Method;
            Path = reqData.Path;
            Version = reqData.Protocol;
            Headers = SanitizeHeaders(reqData.Headers);
            Body = body;
        }

        private static Dictionary<string, string> SanitizeHeaders(Dictionary<string, string> headers)
        {
            var sanitizedHeaders = new Dictionary<string, string>();

            foreach (var header in headers)
            {
                sanitizedHeaders.Add(header.Key.ToUpper(), header.Value);
            }

            return sanitizedHeaders;
        }
    }
}
