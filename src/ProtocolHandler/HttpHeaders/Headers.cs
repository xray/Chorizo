using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Chorizo.ProtocolHandler.ResponseRetriever
{
    public class Headers
    {
        private readonly Header[] _headers;

        public Headers()
        {
            _headers = new Header[0];
        }

        private Headers(Header[] newHeaders)
        {
            _headers = newHeaders;
        }

        public Headers AddHeader(string name, string value)
        {
            var newHeaders = new Header[_headers.Length + 1];
            Array.Copy(_headers, newHeaders, _headers.Length);
            newHeaders[_headers.Length] = new Header(name, value);
            return new Headers(newHeaders);
        }

        public bool ContainsHeader(string name)
        {
            return _headers.Any(header => string.Equals(header.Name(), name, StringComparison.CurrentCultureIgnoreCase));
        }

        public Header GetHeader(string name)
        {
            foreach (var header in _headers)
            {
                if (header.Name() == name) return header;
            }
            throw new KeyNotFoundException();
        }

        public override string ToString()
        {
            return _headers.Aggregate("", (current, header) => current + header.ToString());
        }

        public bool Equals(Headers other)
        {
            if (_headers.Length != other._headers.Length) return false;
            var currentHeader = 0;
            foreach (var header in _headers)
            {
                if (!header.Equals(other._headers[currentHeader])) return false;
                currentHeader++;
            }

            return true;
        }
    }
}
