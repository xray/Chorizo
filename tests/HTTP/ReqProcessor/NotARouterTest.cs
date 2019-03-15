using System;
using System.Runtime.InteropServices;
using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.ReqProcessor;
using Xunit;
using Xunit.Abstractions;

namespace Chorizo.Tests.HTTP.ReqProcessor
{
    public class NotARouterTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public NotARouterTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ProcessTakesInARequestForARouteThatDoesNotExistAndReturnsA404()
        {
            var routes = new Routes()
                .Get("/", req => new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Server", "Chorizo"));

            var router = new NotARouter(routes);
            var request = new Request("GET", "/not-to-be-found", "HTTP/1.1");
            var result = router.HandleRequest(request);

            Assert.Equal("Not Found", result.StatusText);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void ProcessReturnsTheResponseFromTheRouteWithGivenPath() {
            var routes = new Routes()
                .Get("/test", req => new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Server", "Chorizo"));

            var router = new NotARouter(routes);
            var request = new Request("GET", "/test", "HTTP/1.1");
            var result = router.HandleRequest(request);

            Assert.Equal("Chorizo", result.GetHeader("Server").Value);
            Assert.Equal(200, result.StatusCode);
        }

        public void ProcessReturnsTheResponseForTheGivenPathAndHTTPMethod()
        {
            
        }
    }
}
