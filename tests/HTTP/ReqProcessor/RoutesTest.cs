using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.ReqProcessor;
using Xunit;

namespace Chorizo.Tests.HTTP.ReqProcessor
{
    public class RoutesTest
    {
        [Fact]
        public void GetTakesInAPathAndActionAndReturnsANewRoutesObjectWithAGetRouteForTheSpecifiedPath()
        {
            var testRoutes = new Routes();

            var resultingRoutes = testRoutes.Get("/", req => new Response("HTTP/1,1", 200, "OK"));

            var testMatchingRoute = resultingRoutes.RetrieveRoute("GET", "/");

            Assert.True(resultingRoutes.HasMatchingRoute("GET", "/"));
            Assert.Equal("GET", testMatchingRoute.Method);
            Assert.Equal("/", testMatchingRoute.Path);
            Assert.IsType<Action>(testMatchingRoute.Go);
        }
    }
}
