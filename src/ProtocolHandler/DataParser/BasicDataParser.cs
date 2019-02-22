using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chorizo.ProtocolHandler.DataParser
{
    public class BasicDataParser:IDataParser
    {
        public Request Parse(byte[] startLineAndHeadersBytes)
        {
            var rawRequestString = Encoding.UTF8.GetString(startLineAndHeadersBytes);
            var requestLines = rawRequestString.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var reqStartLine = requestLines[0].Split();
            var reqHeadersLines = requestLines.Skip(1).ToArray();
            var reqHeaders = new Dictionary<string, string>();
            foreach (var header in reqHeadersLines)
            {
                var keyAndValue = header.Split(": ");
                reqHeaders.Add(keyAndValue[0].ToUpper(), keyAndValue[1]);
            }

            var method = reqStartLine[0];
            var path = reqStartLine[1];
            var protocol = reqStartLine[2];
            return new Request(method, path, protocol, reqHeaders);
        }
    }
}
