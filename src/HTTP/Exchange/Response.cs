using System.Text;

namespace Chorizo.HTTP.Exchange
{
    public struct Response
    {
        public readonly string Protocol;
        public readonly int StatusCode;
        public readonly string StatusText;
        private readonly Headers _headers;

        public Response(string protocol, int statusCode, string statusText)
        {
            Protocol = protocol;
            StatusCode = statusCode;
            StatusText = statusText;
            _headers = new Headers();
        }

        private Response(string protocol, int statusCode, string statusText, Headers headers)
        {
            Protocol = protocol;
            StatusCode = statusCode;
            StatusText = statusText;
            _headers = headers;
        }

        public Response AddHeader(string name, string value)
        {
            var newHeaders = _headers.AddHeader(name, value);
            return new Response(Protocol, StatusCode, StatusText, newHeaders);
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
            outputString += $"{Protocol} {StatusCode} {StatusText}\r\n";
            outputString += _headers.ToString();
            outputString += "\r\n";
            return outputString;
        }

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }

        public bool Equals(Response other)
        {
            return Protocol == other.Protocol &&
                   StatusCode == other.StatusCode &&
                   StatusText == other.StatusText &&
                   _headers.Equals(other._headers);
        }
    }
}
