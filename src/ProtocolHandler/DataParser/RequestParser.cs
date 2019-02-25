using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Chorizo.ProtocolHandler.DataParser
{
    public class RequestParser:IDataParser
    {
        public Request Parse(byte[] requestBytes)
        {
            var requestString = Encoding.UTF8.GetString(requestBytes);
            var requestLineInfo = GetRequestLineInfo(requestString);
            var requestHeaders = GetRequestHeaders(requestString);
            var reqMethod = GetRequestMethod(requestLineInfo);
            var reqPath = GetRequestPath(requestLineInfo);
            var reqProtocol = GetRequestProtocol(requestLineInfo);
            var request = new Request(reqMethod, reqPath, reqProtocol, requestHeaders);
            return request;
        }

        private static string[] GetRequestLineInfo(string requestString)
        {
            var requestLine = requestString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)[0];
            return requestLine.Split(" ");
        }

        private static Dictionary<string, string> GetRequestHeaders(string requestString)
        {
            var headers = new Dictionary<string, string>();
            var requestLines = requestString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var headerLines = requestLines.Skip(1);
            foreach (var headerString in headerLines)
            {
                var headerKeyAndValue = headerString.Split(": ");
                var key = headerKeyAndValue[0].ToUpper();
                var value = headerKeyAndValue[1];
                headers.Add(key, value);
            }

            return headers;
        }

        private static string GetRequestMethod(IReadOnlyList<string> requestLine)
        {
            return requestLine[0];

        }

        private static string GetRequestPath(IReadOnlyList<string> requestLine)
        {
            return requestLine[1];
        }

        private static string GetRequestProtocol(IReadOnlyList<string> requestLine)
        {
            return requestLine[2];
        }
    }
}
