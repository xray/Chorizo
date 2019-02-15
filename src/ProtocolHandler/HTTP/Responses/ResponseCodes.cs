using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.HTTP.Responses
{
    public class ResponseCodes
    {
        private readonly Dictionary<int, string> _codeReference = new Dictionary<int, string>();

        public ResponseCodes()
        {
            populateReference();
        }

        public string getMessage(int code)
        {
            return _codeReference[code];
        }

        private void populateReference()
        {
            _codeReference.Add(200, "OK");
            _codeReference.Add(400, "Bad Request");
            _codeReference.Add(404, "Not Found");
            _codeReference.Add(500, "Internal Server Error");
        }
    }
}
