namespace Chorizo.ProtocolHandler.ResponseRetriever
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

        public bool Equals(Header other)
        {
            return _name.Equals(other.Name()) && _value.Equals(other.Value());
        }
    }
}
