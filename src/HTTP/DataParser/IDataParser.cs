using Chorizo.HTTP.Exchange;

namespace Chorizo.HTTP.DataParser
{
    public interface IDataParser
    {
        Request Parse(byte[] startLineAndHeadersBytes);
    }
}
