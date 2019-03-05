using System.Text;

namespace Chorizo.HTTP.Exchange
{
    public struct Request
    {
        public readonly string Method;
        public readonly string Path;
        public readonly string Protocol;
        private readonly Headers _headers;
        public readonly string Body;

        public Request(string method, string path, string protocol)
        {
            Method = method;
            Path = path;
            Protocol = protocol;
            _headers = new Headers();
            Body = null;
        }

        public Request(string method, string path, string protocol, string body)
        {
            Method = method;
            Path = path;
            Protocol = protocol;
            _headers = new Headers();
            Body = body;
        }

        public Request(string method, string path, string protocol, Headers headers)
        {
            Method = method;
            Path = path;
            Protocol = protocol;
            _headers = headers;
            Body = null;
        }

        public Request(string method, string path, string protocol, Headers headers, string body)
        {
            Method = method;
            Path = path;
            Protocol = protocol;
            _headers = headers;
            Body = body;
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
            var requestString = "";
            requestString += $"{Method} {Path} {Protocol}\r\n";
            requestString += _headers.ToString();
            requestString += "\r\n";
            requestString += Body ?? "";
            return requestString;
        }

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }

        public bool Equals(Request other)
        {
            return Method == other.Method &&
                   Path == other.Path &&
                   Protocol == other.Protocol &&
                   _headers.Equals(other._headers) &&
                   Body == other.Body;
        }
    }
}
