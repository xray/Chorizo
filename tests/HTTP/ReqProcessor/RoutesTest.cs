using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.ReqProcessor;
using Xunit;

namespace Chorizo.Tests.HTTP.ReqProcessor
{
    public class RoutesTest
    {
        [Fact]
        public void GetCreatesARouteForGet()
        {
            var testRoutes = new Routes()
                .Get("/", req => new Response("HTTP/1,1", 200, "OK"));

            var route = testRoutes.RetrieveRoute("GET", "/");
            Assert.Equal("GET", route.Value.HttpMethod);
            Assert.Equal("/", route.Value.Path);
            Assert.IsType<Action>(route.Value.Action);
        }

        [Fact]
        public void PostCreatesARouteForOptions()
        {
            var request = new Request("OPTIONS", "/", "HTTP/1.1");
            var response = new Response("HTTP/1,1", 200, "OK");

            var testRoutes = new Routes()
                .Post("/", req => response);

            var route = testRoutes.RetrieveRoute("OPTIONS", "/");
            Assert.Equal("OPTIONS", route.Value.HttpMethod);
            Assert.Equal("/", route.Value.Path);
            var foundRequest = route.Value.Action.Invoke(request);

            Assert.Equal("OPTIONS,POST", foundRequest.GetHeader("Allow").Value);
        }

        [Fact]
        public void PutFollowingPostCreatesARouteForOptions()
        {
            var request = new Request("OPTIONS", "/", "HTTP/1.1");
            var response = new Response("HTTP/1,1", 200, "OK");

            var testRoutes = new Routes()
                .Post("/", req => response)
                .Put("/", req => response);

            var route = testRoutes.RetrieveRoute("OPTIONS", "/");
            Assert.Equal("OPTIONS", route.Value.HttpMethod);
            Assert.Equal("/", route.Value.Path);
            var foundRequest = route.Value.Action.Invoke(request);
            Assert.Equal("OPTIONS,POST,PUT", foundRequest.GetHeader("Allow").Value);
        }

        [Fact]
        public void RetrieveRouteTest()
        {
            var testRoutes = new Routes()
                .Get("/", req => new Response("HTTP/1,1", 200, "OK"));

            var route = testRoutes.RetrieveRoute("GET", "/");

            Assert.Equal("GET", route.Value.HttpMethod);
            Assert.Equal("/", route.Value.Path);
            Assert.IsType<Action>(route.Value.Action);
        }

        [Fact]
        public void RetrieveRouteReturns404Route()
        {
            var routes = new Routes();

            var route = routes.RetrieveRoute("GET", "/");

            var resultingResponse = route.Value.Action.Invoke(new Request("GET", "/", "HTTP/1.1"));

            Assert.Equal(404, resultingResponse.StatusCode);
            Assert.Equal("Not Found", resultingResponse.StatusText);
            Assert.Equal("HTTP/1.1", resultingResponse.Protocol);
        }

        [Fact]
        public void RetrieveRouteReturns405Route()
        {
            var routes = new Routes()
                .Get("/test", req => new Response("HTTP/1.1", 200, "OK"))
                .Post("/test", req => new Response("HTTP/1.1", 200, "OK"));

            var route = routes.RetrieveRoute("PUT", "/test");

            var resultingResponse = route.Value.Action.Invoke(new Request("PUT", "/test", "HTTP/1.1"));

            Assert.Equal(405, resultingResponse.StatusCode);
            Assert.Equal("Method Not Allowed", resultingResponse.StatusText);
            Assert.Equal("HTTP/1.1", resultingResponse.Protocol);
            Assert.Equal("OPTIONS,HEAD,GET,POST", resultingResponse.GetHeader("Allow").Value);
        }
    }
}
