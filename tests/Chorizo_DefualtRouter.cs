using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoDefualtRouterShould
    {
        [Fact]
        public void MatchShouldTakeInARequestAndPassItToItsHandler()
        {
            var mockMatcher = new Mock<IMatcher>();

            var testRequest = new Request();

            var testRouter = new DefaultRouter()
            {
                Matcher = mockMatcher.Object
            };

            testRouter.Match(testRequest);
            
            mockMatcher.Verify(handler => handler.Match(testRequest));
        }

        [Fact]
        public void GetShouldTakeInAPathAndHandlerAndCreateANewRouteForAGetRequestAtTheSpecifiedPath()
        {
            var mockMatcher = new Mock<IMatcher>();

            var testRouter = new DefaultRouter
            {
                Matcher = mockMatcher.Object
            };

            var testRoute = new Route("GET", "/", (req, res) => { res.Send("Hello World"); });

            Route.Action action = (req, res) => { res.Send("Hello World!"); };
            
            testRouter.Get("/", action);
            
            Assert.Equal(testRouter.Routes[0].HttpMethod, testRoute.HttpMethod);
            Assert.Equal(testRouter.Routes[0].RoutePath, testRoute.RoutePath);
            Assert.Same(testRouter.Routes[0].Go, action);
        }
    }
}