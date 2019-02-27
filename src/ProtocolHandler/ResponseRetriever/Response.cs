using System.Text;

namespace Chorizo.ProtocolHandler.ResponseRetriever
{
    public struct Response
    {
        private readonly string _protocol;
        private readonly int _statusCode;
        private readonly string _statusText;
        private readonly Headers _headers;

        public Response(string protocol, int statusCode, string statusText)
        {
            _protocol = protocol;
            _statusCode = statusCode;
            _statusText = statusText;
            _headers = new Headers();
        }

        private Response(string protocol, int statusCode, string statusText, Headers headers)
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

        public Response AddHeader(string name, string value)
        {
            var newHeaders = _headers.AddHeader(name, value);
            return new Response(_protocol, _statusCode, _statusText, newHeaders);
        }

        public bool ContainsHeader(string name)
        {
            return _headers.ContainsHeader(name);
        }

        public Header GetHeader(string name)
        {
            return _headers.GetHeader(name);
        }

        public override string ToString()
        {
            var outputString = "";
            outputString += $"{_protocol} {_statusCode} {_statusText}\r\n";
            outputString += _headers.ToString();
            outputString += "\r\n";
            return outputString;
        }

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }
    }
}
