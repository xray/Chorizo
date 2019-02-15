using System;
using System.Collections.Generic;
using System.Linq;

namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public class RawRequestParser:IRequestParser
    {
        public ParsedRequestData Parse(string rawStartLineAndHeaders)
        {
            var requestLines = rawStartLineAndHeaders.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var reqStartLine = requestLines[0].Split();
            var reqHeadersLines = requestLines.Skip(1).ToArray();
            var reqHeaders = new Dictionary<string, string>();
            foreach (var header in reqHeadersLines)
            {
                var keyAndValue = header.Split(": ");
                reqHeaders.Add(keyAndValue[0], keyAndValue[1]);
            }

            return new ParsedRequestData
            {
                Method = reqStartLine[0],
                Path = reqStartLine[1],
                Protocol = reqStartLine[2],
                Headers = reqHeaders
            };
        }
    }
}
