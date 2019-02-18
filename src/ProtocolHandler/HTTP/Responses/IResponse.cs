namespace Chorizo.ProtocolHandler.HTTP.Responses
{
    public interface IResponse
    {
        IResponse Append(string field, string value);
        IResponse Status(int responseCode);
        void Send(string message = null, string contentType = "text/html");
    }
}
