using System;
using System.Net.Sockets;
using System.Text;
using Chorizo.Logger;
using Chorizo.Sockets.InternalSocket;

namespace Chorizo.Echo
{
    public class EchoConnectionHandler : IProtocolConnectionHandler
    {
        private readonly IMiniLogger _optionalLogger;
        public EchoConnectionHandler(IMiniLogger optionalLogger = null)
        {
            _optionalLogger = optionalLogger;
        }

        public void HandleRequest(IAppSocket appSocket)
        {
            var retrievedData = retrieveData(appSocket);
            echoData(appSocket, retrievedData);
        }

        private string retrieveData(IAppSocket appSocket)
        {
            var bufferText = "";
            var receivedData = new byte[0];

            while (bufferText.IndexOf("\n") == -1)
            {
                var (data, dataLength) = appSocket.Receive(1);
                var originalLength = receivedData.Length;
                Array.Resize(ref receivedData, originalLength + dataLength);
                Array.Copy(data, 0, receivedData, originalLength, dataLength);
                bufferText = Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);
            }
            _optionalLogger?.Info($"Got Data: {bufferText}");
            return bufferText;
        }

        private void echoData(IAppSocket appSocket, string toEcho)
        {
            byte[] msg = Encoding.ASCII.GetBytes(toEcho);
            appSocket.Send(msg);
            _optionalLogger?.Info("Sent Back the data, closing the connection...");
            appSocket.Disconnect(SocketShutdown.Both);
        }
    }
}
