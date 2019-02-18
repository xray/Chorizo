using Chorizo.ProtocolHandler.HTTP.Requests;
using Chorizo.ProtocolHandler.HTTP.Responses;
using Chorizo.ProtocolHandler.HTTP.Router;
using Moq;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.HTTP.Router
{
    public class ChorizoRouterTest
    {
        private readonly ChorizoRouter _testRouter;
        private readonly Mock<IRequest> _mockRequest;
        private readonly Mock<IResponse> _mockResponse;

        public ChorizoRouterTest()
        {
            _testRouter = new ChorizoRouter();
            _mockRequest = new Mock<IRequest>();
            _mockRequest.Setup(req => req.Path()).Returns("/");
            _mockRequest.Setup(req => req.Method()).Returns("GET");
            _mockResponse = new Mock<IResponse>();
        }
        [Fact]
        public void MatchGivenARequestThatMatchesARouteWillCallTheActionAtThatRoute()
        {
            var mockAction = new Mock<Action>();
            _testRouter.Get("/", mockAction.Object);

            _testRouter.Match(_mockRequest.Object, _mockResponse.Object);

            _mockRequest.Verify(req => req.Path(), Times.Exactly(3));
            _mockRequest.Verify(req => req.Method(), Times.Once);
            mockAction.Verify(act => act(_mockRequest.Object, _mockResponse.Object));
        }

        [Fact]
        public void MatchGivenARequestThatDoesNotMatchARouteRespondWith404NotFound()
        {
            _mockResponse.Setup(res => res.Status(It.IsAny<int>())).Returns(_mockResponse.Object);

            _testRouter.Match(_mockRequest.Object, _mockResponse.Object);

            _mockRequest.Verify(req => req.Path(), Times.Exactly(1));
            _mockResponse.Verify(res => res.Status(404));
            _mockResponse.Verify(res => res.Send("Not Found", "text/html"));
        }
    }
}
