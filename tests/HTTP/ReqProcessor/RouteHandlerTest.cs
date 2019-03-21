using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.ReqProcessor;
using Xunit;

namespace Chorizo.Tests.HTTP.ReqProcessor
{
    public class RouteHandlerTest
    {
        [Fact]
        public void HandleRequestTakesInARequestForARouteThatDoesNotExistAndReturnsA404()
        {
            var routes = new Routes().Get("/", req =>
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Server", "Chorizo");
                }
            );

            var router = new RouteHandler(routes);
            var request = new Request("GET", "/not-to-be-found", "HTTP/1.1");
            var result = router.HandleRequest(request);

            Assert.Equal("Not Found", result.StatusText);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void HandleRequestReturnsTheResponseFromTheRouteWithGivenPath() {
            var routes = new Routes().Get("/test", req =>
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Server", "Chorizo");
                }
            );

            var router = new RouteHandler(routes);
            var request = new Request("GET", "/test", "HTTP/1.1");
            var result = router.HandleRequest(request);

            Assert.Equal("Chorizo", result.GetHeader("Server").Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void HandleRequestReturnsAOptionsResponseForTheGivenPath()
        {
            var routes = new Routes()
                .Get("/test", req =>
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Server", "Chorizo");
                })
                .Post("/test", req =>
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Server", "Chorizo");
                });

            var router = new RouteHandler(routes);
            var request = new Request("OPTIONS", "/test", "HTTP/1.1");
            var result = router.HandleRequest(request);

            Assert.Equal("Chorizo", result.GetHeader("Server").Value);
            Assert.Equal("HEAD,GET,POST,OPTIONS", result.GetHeader("Allow").Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void HandleRequestReturnsNoBodyWhenHeadIsRequestedOnGetEndpoint()
        {
            var routes = new Routes()
                .Get("/test", req =>
                {
                    return new Response("HTTP/1.1", 200, "OK", "Hello World")
                        .AddHeader("Server", "Chorizo");
                });

            var router = new RouteHandler(routes);
            var request = new Request("HEAD", "/test", "HTTP/1.1");
            var result = router.HandleRequest(request);

            Assert.Equal("Chorizo", result.GetHeader("Server").Value);
            Assert.Equal("11", result.GetHeader("Content-Length").Value);
            Assert.Equal("", result.Body);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void HandleRequestReturns405WhenMethodDoesNotExistAtRoute()
        {
            var routes = new Routes()
                .Get("/test", req =>
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Server", "Chorizo");
                }).Post("/test", req =>
                {
                    return new Response("HTTP/1.1", 200, "OK")
                        .AddHeader("Server", "Chorizo");
                });

            var router = new RouteHandler(routes);
            var request = new Request("PUT", "/test", "HTTP/1.1");
            var result = router.HandleRequest(request);

            Assert.Equal("HEAD,GET,POST,OPTIONS", result.GetHeader("Allow").Value);
            Assert.Equal("Chorizo", result.GetHeader("Server").Value);
            Assert.Equal(405, result.StatusCode);
        }
    }
}
