using Chorizo.HTTP.Exchange;
using Chorizo.Logger;

namespace Chorizo.HTTP.ReqProcessor
{
    public class RequestProcessor:IRequestProcessor
    {
        private readonly string _currentTime;

        public RequestProcessor(IDateTimeProvider dateTimeProvider)
        {
            _currentTime = $"{dateTimeProvider.Now():R}";
        }

        public Response Process(Request req)
        {
            switch (req.Path)
            {
                case "/simple_get":
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Date", _currentTime);
                }
                case "/echo_body":
                {
                    return new Response("HTTP/1.1", 200, "OK", req.Body)
                        .AddHeader("Date", _currentTime);
                }
                default:
                {
                    return new Response("HTTP/1.1", 404, "Not Found")
                        .AddHeader("Date", _currentTime);
                }
            }
        }
    }
}
