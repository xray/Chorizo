using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.ReqProcessor;
using Xunit;

namespace Chorizo.Tests.HTTP.ReqProcessor
{
    public class NotARouterTest
    {
        [Fact]
        public void ProcessTakesInARequestForARouteThatDoesNotExistAndReturnsA404()
        {
            var routes = new Routes()
                .Get("/", req => new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Server", "Chorizo"));

            var router = new NotARouter(routes);
            var request = new Request("GET", "", "HTTP/1.1");
            var response = new Response("HTTP/1.1", 404, "Not Found");
            var result = router.Process(request);

            Assert.Equal(response, result);
        }
    }
}
