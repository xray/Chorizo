using System.Text;

namespace Chorizo.HTTP.Exchange
{
    public readonly struct Response
    {
        public readonly string Protocol;
        public readonly int StatusCode;
        public readonly string StatusText;
        private readonly Headers _headers;
        public readonly string Body;

        private Response(string protocol, int statusCode, string statusText, Headers headers, string body)
        {
            Protocol = protocol;
            StatusCode = statusCode;
            StatusText = statusText;
            _headers = headers;
            Body = body;
        }

        public Response(string protocol, int statusCode, string statusText) : this(protocol, statusCode, statusText, new Headers(), ""){}

        public Response(string protocol, int statusCode, string statusText, string body): this(
            protocol,
            statusCode,
            statusText,
            new Headers()
                .AddHeader("Content-Length", Encoding.UTF8.GetBytes(body).Length.ToString()),
            body
        ){}

        public Response AddHeader(string name, string value)
        {
            var newHeaders = _headers.AddHeader(name, value);
            return new Response(Protocol, StatusCode, StatusText, newHeaders, Body);
        }

        public bool ContainsHeader(string name)
        {
            return _headers.ContainsHeader(name);
        }

        public Header GetHeader(string name)
        {
            return _headers[name];
        }

        public override string ToString()
        {
            var outputString = "";
            outputString += $"{Protocol} {StatusCode} {StatusText}\r\n";
            outputString += _headers.ToString();
            outputString += "\r\n";
            outputString += Body;
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
                   _headers.Equals(other._headers) &&
                   Body == other.Body;
        }
    }
}
