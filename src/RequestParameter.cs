namespace Chorizo
{
    public class RequestParameter
    {
        private string _key;
        private string _value;

        public RequestParameter(string key, string value)
        {
            _key = key;
            _value = value;
        }
    }
}