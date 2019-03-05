using System.Net.Sockets;
using Chorizo.HTTP.DataParser;
using Chorizo.HTTP.ReqProcessor;
using Chorizo.HTTP.SocketReader;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.HTTP {
    public class HttpConnectionHandler : IProtocolConnectionHandler {

        public ISocketReader SocketReader;
        public IDataParser DataParser { get; set; }
        public IRequestProcessor RequestProcessor { get; set; }

        public void HandleRequest(IChorizoSocket socket) {
            var bytes = SocketReader.ReadSocket(socket);
            var request = DataParser.Parse(bytes);
            var response = RequestProcessor.Process(request);
            socket.Send(response.ToByteArray());
            socket.Disconnect(SocketShutdown.Both);
        }
    }
}
