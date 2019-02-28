using System.Text;
using Chorizo.ProtocolHandler.HttpHeaders;

namespace Chorizo.ProtocolHandler.DataParser
{
    public struct Request
    {
        private readonly string _method;
        private readonly string _path;
        private readonly string _protocol;
        private readonly Headers _headers;

        public Request(string method, string path, string protocol)
        {
            _method = method;
            _path = path;
            _protocol = protocol;
            _headers = new Headers();
        }

        public Request(string method, string path, string protocol, Headers headers)
        {
            _method = method;
            _path = path;
            _protocol = protocol;
            _headers = headers;
        }

        public string Method()
        {
            return _method;
        }

        public string Path()
        {
            return _path;
        }

        public string Protocol()
        {
            return _protocol;
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
            requestString += $"{Method()} {Path()} {Protocol()}\r\n";
            requestString += _headers.ToString();
            requestString += "\r\n";
            return requestString;
        }

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }

        public bool Equals(Request other)
        {
            return Method() == other.Method() &&
                   Path() == other.Path() &&
                   Protocol() == other.Protocol() &&
                   _headers.Equals(other._headers);
        }
    }
}
