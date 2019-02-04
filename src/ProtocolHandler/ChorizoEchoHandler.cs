using System;
using System.Net.Sockets;
using System.Text;
using Chorizo.Sockets.CzoSocket;

namespace Chorizo.ProtocolHandler
{
    public class ChorizoEchoHandler : IChorizoProtocolHandler
    {
        private readonly string _protocol;

        public ChorizoEchoHandler()
        {
            _protocol = "TelNet";
        }
        public bool WillHandle(string protocol)
        {
            return protocol == _protocol;
        }

        public void Handle(IChorizoSocket chorizoSocket)
        {
            var retrievedData = retrieveData(chorizoSocket);
            echoData(chorizoSocket, retrievedData);
        }

        private string retrieveData(IChorizoSocket chorizoSocket)
        {
            var bufferText = "";
            var receivedData = new byte[0];
            
            while (bufferText.IndexOf("\n") == -1)
            {
                var (data, dataLength) = chorizoSocket.Receive(1);
                var originalLength = receivedData.Length;
                Array.Resize(ref receivedData, originalLength + dataLength);
                Array.Copy(data, 0, receivedData, originalLength, dataLength);
                bufferText = Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);
            }
            Console.WriteLine($"Got Data: {bufferText}");
            return bufferText;
        }

        private void echoData(IChorizoSocket chorizoSocket, string toEcho)
        {
            byte[] msg = Encoding.ASCII.GetBytes(toEcho);
            chorizoSocket.Send(msg);
            Console.WriteLine("Sent Back the data, closing the connection...");
            chorizoSocket.Shutdown(SocketShutdown.Both);
            chorizoSocket.Close();
        }
    }
}