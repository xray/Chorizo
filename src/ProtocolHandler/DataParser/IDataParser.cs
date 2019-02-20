namespace Chorizo.ProtocolHandler.DataParser
{
    public interface IDataParser
    {
        Request Parse(byte[] startLineAndHeadersBytes);
    }
}
