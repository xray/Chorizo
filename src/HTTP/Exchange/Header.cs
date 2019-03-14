using System;

namespace Chorizo.HTTP.Exchange
{
    public struct Header
    {
        public readonly string Name;
        public readonly string Value;

        public Header(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}: {Value}\r\n";
        }

        public bool Equals(Header other)
        {
            return string.Equals(Name, other.Name, StringComparison.CurrentCultureIgnoreCase) &&
                   Value == other.Value;
        }
   }
}
