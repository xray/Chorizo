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

        public Response Retrieve(Request req)
        {
            if (req.Path == "/simple_get")
            {
                return new Response("HTTP/1.1", 200, "OK")
                    .AddHeader("Date", _currentTime);
            }

            return new Response("HTTP/1.1", 404, "Not Found")
                .AddHeader("Date", _currentTime);
        }
    }
}
