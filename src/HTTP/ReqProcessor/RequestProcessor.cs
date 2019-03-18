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

        public Response HandleRequest(Request req)
        {
            switch (req.Path)
            {
                case "/simple_get":
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Date", _currentTime);
                case "/echo_body":
                    return new Response("HTTP/1.1", 200, "OK", req.Body)
                        .AddHeader("Date", _currentTime);
                case "/method_options":
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Date", _currentTime)
                        .AddHeader("Allow", "GET,HEAD,OPTIONS");
                case "/method_options2":
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Date", _currentTime)
                        .AddHeader("Allow", "GET,HEAD,OPTIONS,PUT,POST");
                case "/redirect":
                    return new Response("HTTP/1.1", 301, "Moved Permanently")
                        .AddHeader("Date", _currentTime)
                        .AddHeader("Location", "http://localhost:5000/simple_get");
                case "/get_with_body":
                    if (req.Method == "HEAD")
                    {
                        return new Response("HTTP/1.1", 200, "OK")
                            .AddHeader("Date", _currentTime);
                    }
                    return new Response("HTTP/1.1", 405, "Method Not Allowed")
                        .AddHeader("Date", _currentTime)
                        .AddHeader("Allow", "HEAD,OPTIONS");
                default:
                    return new Response("HTTP/1.1", 404, "Not Found")
                        .AddHeader("Date", _currentTime);
            }
        }
    }
}
