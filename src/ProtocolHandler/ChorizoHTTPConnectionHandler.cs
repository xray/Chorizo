using Chorizo.ProtocolHandler.DataParser;
using Chorizo.ProtocolHandler.ResponseRetriever;
using Chorizo.Sockets.CzoSocket;
using Chorizo.ProtocolHandler.SocketReader;

namespace Chorizo.ProtocolHandler{


    public interface IResponseSender
    {
        byte[] Send(IChorizoSocket socket, Response res);
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
