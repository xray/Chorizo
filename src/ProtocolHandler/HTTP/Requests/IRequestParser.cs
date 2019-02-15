namespace Chorizo.ProtocolHandler.HTTP.Requests
{
    public interface IRequestParser
    {
        ParsedRequestData Parse(string rawStartLineAndHeaders);
    }
}
