using System;
using System.Text;
using Chorizo.Logger;
using Chorizo.ProtocolHandler.HTTP.Requests;
using Chorizo.ProtocolHandler.HTTP.Responses;
using Chorizo.ProtocolHandler.HTTP.Router;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler.HTTP
{
    public class ChorizoHTTPConnectionHandler:IChorizoProtocolConnectionHandler
    {
        private IRequestParser _rawRequestParser;
        private IRouter _router;
        private readonly IMiniLogger _optionalLogger;

        public ChorizoHTTPConnectionHandler(
            IMiniLogger optionalLogger = null,
            IRequestParser rawRequestParser = null,
            IRouter router = null
        )
        {
            _router = router ?? new ChorizoRouter();
            _rawRequestParser = rawRequestParser ?? new RawRequestParser();
            _optionalLogger = optionalLogger;
        }

        public void HandleRequest(IChorizoSocket chorizoSocket)
        {
            var rawStartLineAndHeaders = receiveReqStartLineAndHeaders(chorizoSocket);
            if (rawStartLineAndHeaders.Length <= 1)
            {
                chorizoSocket.Disconnect();
            }
            else
            {
                var parsedRequestInfo = _rawRequestParser.Parse(rawStartLineAndHeaders);
                var contentLength = 0;
                if (parsedRequestInfo.Headers.ContainsKey("CONTENT-LENGTH"))
                {
                    int.TryParse(parsedRequestInfo.Headers["CONTENT-LENGTH"], out contentLength);
                }
                var requestBody = receiveReqBody(chorizoSocket, contentLength);
                var incomingRequest = new Request(parsedRequestInfo, requestBody);
                var outgoingResponse = new Response(chorizoSocket);
                _router.Match(incomingRequest, outgoingResponse);
            }
        }

        public IRouter Router()
        {
            return _router;
        }

        private string receiveReqStartLineAndHeaders(IChorizoSocket chorizoSocket)
        {
            var startLineAndHeaders = "";
            var receivedData = new byte[0];

            while (startLineAndHeaders.IndexOf("\r\n\r\n") == -1)
            {
                var (data, bytesReceived) = chorizoSocket.Receive(1);
                if (bytesReceived == 0 || receivedData.Length == 8000)
                {
                    startLineAndHeaders = "";
                    break;
                }
                var originalLength = receivedData.Length;
                Array.Resize(ref receivedData, originalLength + 1);
                Array.Copy(data, 0, receivedData, originalLength, 1);
                startLineAndHeaders = Encoding.UTF8.GetString(receivedData, 0, receivedData.Length);
            }
            return startLineAndHeaders;
        }

        private byte[] receiveReqBody(IChorizoSocket chorizoSocket, int contentLength)
        {
            if (contentLength <= 0) return null;
            var (data, _) = chorizoSocket.Receive(contentLength);
            return data;
        }
    }
}
