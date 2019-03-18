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
            var testRoutes = new Routes()
                .Get("/", req => new Response("HTTP/1,1", 200, "OK"));

            var route = testRoutes.RetrieveRoute("GET", "/");

            Assert.Equal("GET", route.Value.HttpMethod);
            Assert.Equal("/", route.Value.Path);
            Assert.IsType<Action>(route.Value.Action);
        }


        // TODO: write a negative assertion for null case

        [Fact]
        public void GetAvailableMethodsReturnsAResponseWithAllowHeaderListingAvailableMethods()
        {
            var testRoutes = new Routes()
                .Get("/", req => new Response("HTTP/1.1", 200, "OK"))
                .Post("/", req => new Response("HTTP/1.1", 200, "OK"));

            var methods = testRoutes.GetAvailableMethods("/");

            Assert.Equal("GET,HEAD,POST,OPTIONS", methods);
        }
    }
}
