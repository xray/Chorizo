using System.Collections.Generic;
using Chorizo.ProtocolHandler.DataParser;
using Chorizo.Sockets.CzoSocket;
using Chorizo.ProtocolHandler.SocketReader;

namespace Chorizo.ProtocolHandler{
    public interface IResponseRetriever
    {
        Response Retrieve(Request req);
    }

    public interface IResponseSender
    {
        byte[] Send(IChorizoSocket socket, Response res);
    }

    public struct Response
    {
        public readonly string Method;
        public readonly int StatusCode;
        public readonly string StatusText;
        public readonly Dictionary<string, string> Headers;

        public Response(string method, int statusCode, string statusText, Dictionary<string, string> headers)
        {
            Method = method;
            StatusCode = statusCode;
            StatusText = statusText;
            Headers = headers;
        }
    }

    public class ChorizoHttpConnectionHandler {

        public IHTTPSocketReader HttpSocketReader;
        public IDataParser DataParser { get; set; }
        public IResponseRetriever ResponseRetriever { get; set; }
        public IResponseSender ResponseSender { get; set; }

        public void HandleRequest(IChorizoSocket socket) {
            var bytes = HttpSocketReader.ReadSocket(socket);
            var request = DataParser.Parse(bytes);
            var response = ResponseRetriever.Retrieve(request);
            ResponseSender.Send(socket, response);
        }
    }
}
