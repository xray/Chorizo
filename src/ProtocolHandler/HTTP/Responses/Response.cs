using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chorizo.Date;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler.HTTP.Responses
{
    public class Response : IResponse
    {
        private readonly IChorizoSocket _socket;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ResponseCodes _responseCodes;
        private string _protocol = "HTTP/1.1";
        private int _statusCode = 200;
        private string _statusText = "OK";
        private readonly Dictionary<string, string> _headers;
        private readonly Dictionary<string, string> _additionalHeaders;

        public Response(IChorizoSocket chorizoSocket, IDateTimeProvider dateTimeProvider = null)
        {
            _socket = chorizoSocket;
            _dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();
            _headers = new Dictionary<string, string>();
            _additionalHeaders = new Dictionary<string, string>();
            _responseCodes = new ResponseCodes();
        }

        public IResponse Append(string field, string value)
        {
            _additionalHeaders.Add(field, value);
            return this;
        }

        public IResponse Status(int responseCode)
        {
            _statusCode = responseCode;
            _statusText = _responseCodes.getMessage(responseCode);
            return this;
        }

        public void Send(string message = null, string contentType = "text/html")
        {
            _headers.Add("Connection", "Closed");
            _headers.Add("date", _dateTimeProvider.Now().ToString("R"));
            _headers.Add("Server", "Chorizo");
            if (message != null)
            {
                var encodedMessage = Encoding.UTF8.GetBytes(message);
                _headers.Add("Content-Length", encodedMessage.Length.ToString());
                _headers.Add("Content-Type", contentType);
            }

            _additionalHeaders.ToList().ForEach(x => _headers[x.Key] = x.Value);

            var formattedResponse = "";

            formattedResponse += $"{_protocol} {_statusCode} {_statusText}\r\n";
            foreach (var (key, value) in _headers)
            {
                formattedResponse += $"{key}: {value}\r\n";
            }

            formattedResponse += "\r\n";
            if (message != null)
            {
                formattedResponse += $"{message}";
            }
            var encodedResponse = Encoding.UTF8.GetBytes(formattedResponse);
            _socket.Send(encodedResponse);
            _socket.Disconnect();
        }

        public bool Equals(Response other)
        {
            return _socket == other._socket;
        }
    }
}
