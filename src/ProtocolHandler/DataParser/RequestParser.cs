using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chorizo.ProtocolHandler.ResponseRetriever;

namespace Chorizo.ProtocolHandler.DataParser
{
    public class RequestParser:IDataParser
    {
        public Request Parse(byte[] requestBytes)
        {
            var requestString = Encoding.UTF8.GetString(requestBytes);
            var requestLineInfo = GetRequestLineInfo(requestString);
            var reqMethod = GetRequestMethod(requestLineInfo);
            var reqPath = GetRequestPath(requestLineInfo);
            var reqProtocol = GetRequestProtocol(requestLineInfo);
            var requestHeaders = GetRequestHeaders(requestString);
            var request = new Request(reqMethod, reqPath, reqProtocol, requestHeaders);
            return request;
        }

        private static string[] GetRequestLineInfo(string requestString)
        {
            var requestLine = requestString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)[0];
            return requestLine.Split(" ");
        }

        private static Headers GetRequestHeaders(string requestString)
        {
            var headers = new Headers();
            var requestLines = requestString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var headerLines = requestLines.Skip(1);
            Console.WriteLine($"this: {requestString}");
            foreach (var headerString in headerLines)
            {
                var headerKeyAndValue = headerString.Split(": ");
                var name = headerKeyAndValue[0];
                var value = headerKeyAndValue[1];
                headers = headers.AddHeader(name, value);
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
