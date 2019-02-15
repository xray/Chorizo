using System;
using System.Collections.Generic;
using System.Text;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler.HTTP.Responses
{
    public class Response
    {
        private IChorizoSocket _socket;
        private ResponseCodes _responseCodes;
        private string _protocol = "HTTP/1.1";
        private int _statusCode = 200;
        private string _statusText = "OK";
        private Dictionary<String, String> _headers;

        public Response(IChorizoSocket chorizoSocket)
        {
            _socket = chorizoSocket;
            _headers = new Dictionary<string, string>();
            _headers.Add("Server", "Chorizo");
            _responseCodes = new ResponseCodes();
        }

        public void Append(string field, string value)
        {
            _headers.Add(field, value);
        }

        public Response Status(int responseCode)
        {
            _statusCode = responseCode;
            _statusText = _responseCodes.getMessage(responseCode);
            return this;
        }

        public void Send(string message = null, string contentType = "text/html")
        {
            if (message != null)
            {
                _headers.Add("Content-Type", contentType);
                var encodedMessage = Encoding.UTF8.GetBytes(message);
                _headers.Add("Content-Length", encodedMessage.Length.ToString());
            }
            _headers.Add("date", DateTime.Now.ToString("R"));
            _headers.Add("Connection", "Closed");

            var formattedResponse = "";

            formattedResponse += $"{_protocol} {_statusCode} {_statusText}\r\n";
            foreach (var header in _headers)
            {
                formattedResponse += $"{header.Key}: {header.Value}\r\n";
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
    }
}
