using System;

namespace Chorizo.ProtocolHandler.ResponseRetriever
{
    public struct Response
    {
        private readonly string _protocol;
        private readonly int _statusCode;
        private readonly string _statusText;
        private readonly Header[] _headers;

        public Response(string protocol, int statusCode, string statusText)
        {
            _protocol = protocol;
            _statusCode = statusCode;
            _statusText = statusText;
            _headers = new Header[0];
        }

        private Response(string protocol, int statusCode, string statusText, Header[] headers)
        {
            _protocol = protocol;
            _statusCode = statusCode;
            _statusText = statusText;
            _headers = headers;
        }

        public string Protocol()
        {
            return _protocol;
        }

        public int StatusCode()
        {
            return _statusCode;
        }

        public string StatusText()
        {
            return _statusText;
        }

        public Header[] Headers()
        {
            return _headers;
        }

        public Response AddHeader(string name, string value)
        {
            var newHeaders = new Header[_headers.Length + 1];
            Array.Copy(_headers, newHeaders, _headers.Length);
            newHeaders[_headers.Length] = new Header(name, value);
            return new Response(_protocol, _statusCode, _statusText, newHeaders);
        }
    }
}
