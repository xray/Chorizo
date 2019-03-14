using System.Net.Sockets;
using Chorizo.HTTP.DataParser;
using Chorizo.HTTP.ReqProcessor;
using Chorizo.HTTP.SocketReader;
using Chorizo.Sockets.InternalSocket;

namespace Chorizo.HTTP {
    public class HttpConnectionHandler : IProtocolConnectionHandler {

        public ISocketReader SocketReader;
        public IDataParser DataParser { get; set; }
        public IRequestProcessor RequestProcessor { get; set; }

        public void HandleRequest(IAppSocket socket) {
            var bytes = SocketReader.ReadSocket(socket);
            var request = DataParser.Parse(bytes);
            var requestWithBody = SocketReader.ReadBody(socket, request);
            var response = RequestProcessor.Process(requestWithBody);
            socket.Send(response.ToByteArray());
            socket.Disconnect(SocketShutdown.Both);
        }
    }
}
