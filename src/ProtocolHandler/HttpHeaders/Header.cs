using System;

namespace Chorizo.ProtocolHandler.HttpHeaders
{
    public struct Header
    {
        private readonly string _name;
        private readonly string _value;

        public Header(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string Name()
        {
            return _name;
        }

        public string Value()
        {
            return _value;
        }

        public override string ToString()
        {
            return $"{_name}: {_value}\r\n";
        }

        public bool Equals(Header other)
        {
            return string.Equals(Name(), other.Name(), StringComparison.CurrentCultureIgnoreCase) &&
                   Value() == other.Value();
        }
   }
}
