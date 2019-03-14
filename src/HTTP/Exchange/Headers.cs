using System;
using System.Collections.Generic;
using System.Linq;

namespace Chorizo.HTTP.Exchange
{
    public class Headers
    {
        private readonly Header[] _headers;

        public Headers()
        {
            _headers = new Header[0];
        }

        public Headers(string name, string value)
        {
            _headers = new []{new Header(name, value)};
        }

        private Headers(Header[] newHeaders)
        {
            _headers = newHeaders;
        }

        public Header this[string name]
        {
            get
            {
                foreach (var header in _headers)
                {
                    if (header.Name == name) return header;
                }
                throw new KeyNotFoundException();
            }
        }

        public bool ContainsHeader(string name)
        {
            return _headers.Any(header => string.Equals(header.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public Headers AddHeader(string name, string value)
        {
            Header[] newHeaders;
            if (ContainsHeader(name))
            {
                newHeaders = ReplaceHeader(name, value);
            }
            else
            {
                newHeaders = new Header[_headers.Length + 1];
                Array.Copy(_headers, newHeaders, _headers.Length);
                newHeaders[_headers.Length] = new Header(name, value);
            }

            return new Headers(newHeaders);
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

        private Header[] ReplaceHeader(string name, string value)
        {
            var updatedHeaders = new Header[_headers.Length];
            var currentLocation = 0;
            foreach (var header in _headers)
            {
                if (header.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    updatedHeaders[currentLocation] = new Header(name, value);
                }
                else
                {
                    updatedHeaders[currentLocation] = new Header(header.Name, header.Value);
                }

                currentLocation++;
            }

            return updatedHeaders;
        }
    }
}
