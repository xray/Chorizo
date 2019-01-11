using Xunit;
using Moq;

namespace Chorizo.Tests
{
    public class ChorizoDefualtRouterShould
    {
        [Fact]
        public void GetShouldTakeInAPathAndHandlerAndCreateANewRouteForAGetRequestAtTheSpecifiedPath()
        {
            // Arrange
            var mockMatcher = new Mock<IMatcher>();

            var testRouter = new DefaultRouter
            {
                Matcher = mockMatcher.Object
            };

            DefaultRouter.Action action = (req, res) => { res.Send("Hello World!"); };
            
            // Act
            testRouter.Get("/", action);
            
            // Assert
            Assert.Equal(testRouter.Routes[0].HttpMethod, "GET");
            Assert.Equal(testRouter.Routes[0].RoutePath, "/");
            Assert.Same(testRouter.Routes[0].Go, action);
        }
        
        [Fact]
        public void MatchShouldTakeInARequestMethodAndRequestPathAndReturnTrueIfARouteExistsThatMatchesTheParams()
        {
            // Arrange
            var testRequest = new Request
            {
                Method = "GET",
                Path = "/"
            };
            var testRouter = new DefaultRouter();
            
            // Act
            testRouter.Get("/", (req, res) => { res.Send("Hello World!"); });
            
            // Assert
            Assert.True(testRouter.Match(testRequest.Method, testRequest.Path));
        }
    }
}