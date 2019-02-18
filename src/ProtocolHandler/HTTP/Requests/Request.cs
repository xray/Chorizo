using System.Collections.Generic;

namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public class Request:IRequest
    {
        private readonly string _method;
        private readonly string _path;
        private readonly string _protocol;
        private readonly Dictionary<string, string> _headers;
        private readonly byte[] _body;

        public Request(ParsedRequestData reqData, byte[] body)
        {
            _method = reqData.Method;
            _path = reqData.Path;
            _protocol = reqData.Protocol;
            _headers = reqData.Headers;
            _body = body;
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

        public Dictionary<string, string> Headers()
        {
            return _headers;
        }

        public byte[] Body()
        {
            return _body;
        }

        public bool Equals(Request other)
        {
            var mppMatch = Method() == other.Method() &&
                           Path() == other.Path() &&
                           Protocol() == other.Protocol();

            if (!mppMatch) return false;

            foreach (var (key, value) in Headers())
            {
                if (!other.Headers().ContainsKey(key)) return false;
                if (other.Headers()[key] != value) return false;
            }

            return Body() == other.Body();
        }
    }
}
