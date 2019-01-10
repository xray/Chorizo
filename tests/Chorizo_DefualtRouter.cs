using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoDefualtRouterShould
    {
        [Fact]
        public void MatchShouldTakeInARequestAndPassItToItsHandler()
        {
            // Arrange
            var mockMatcher = new Mock<IMatcher>();

            var testRequest = new Request();

            var testRouter = new DefaultRouter()
            {
                Matcher = mockMatcher.Object
            };
            
            // Act
            testRouter.Match(testRequest);
            
            // Assert
            mockMatcher.Verify(handler => handler.Match(testRequest));
        }

        [Fact]
        public void GetShouldTakeInAPathAndHandlerAndCreateANewRouteForAGetRequestAtTheSpecifiedPath()
        {
            // Arrange
            var mockMatcher = new Mock<IMatcher>();

            var testRouter = new DefaultRouter
            {
                Matcher = mockMatcher.Object
            };

            Route.Action action = (req, res) => { res.Send("Hello World!"); };
            
            // Act
            testRouter.Get("/", action);
            
            // Assert
            Assert.Equal(testRouter.Routes[0].HttpMethod, "GET");
            Assert.Equal(testRouter.Routes[0].RoutePath, "/");
            Assert.Same(testRouter.Routes[0].Go, action);
        }
    }
}