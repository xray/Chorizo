using System;
using System.Net.Sockets;
using Chorizo.ProtocolHandler.DataParser;
using Chorizo.ProtocolHandler.ResponseRetriever;
using Chorizo.Sockets.CzoSocket;
using Chorizo.ProtocolHandler.SocketReader;

namespace Chorizo.ProtocolHandler{
    public class ChorizoHttpConnectionHandler {

        public IHTTPSocketReader HttpSocketReader;
        public IDataParser DataParser { get; set; }
        public IResponseRetriever ResponseRetriever { get; set; }

        public void HandleRequest(IChorizoSocket socket) {
            var bytes = HttpSocketReader.ReadSocket(socket);
            var request = DataParser.Parse(bytes);
            var response = ResponseRetriever.Retrieve(request);
            socket.Send(response.ToByteArray());
            socket.Disconnect(SocketShutdown.Both);
        }
    }
}
