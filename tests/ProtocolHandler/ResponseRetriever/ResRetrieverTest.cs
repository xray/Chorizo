using System;
using System.Collections.Generic;
using Chorizo.Logger;
using Chorizo.ProtocolHandler.DataParser;
using Chorizo.ProtocolHandler.ResponseRetriever;
using Moq;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.ResponseRetriever
{
    public class ResRetrieverTest
    {
        private readonly Mock<IDateTimeProvider> _mockDateTime;
        public ResRetrieverTest()
        {
            _mockDateTime = new Mock<IDateTimeProvider>();
            var testTime = new DateTime(1997, 12, 02, 15, 10, 00, DateTimeKind.Utc);
            _mockDateTime.Setup(dt => dt.Now()).Returns(testTime);
        }

        [Fact]
        public void RetrieveTakesARequestForForwardSlashSimpleGetAndReturnsAResponseOfTwoHundredOK()
        {
            var testRequest = new Request(
                "GET",
                "/simple_get",
                "HTTP/1.1"
            );

            var testResponse = new Response(
                "HTTP/1.1",
                200,
                "OK",
                new Dictionary<string, string>
                {
                    {"Date", "Tue, 02 Dec 1997 15:10:00 GMT"}
                }
            );

            var testResponseRetriever = new ResRetriever
            {
                DateTimeProvider = _mockDateTime.Object
            };

            var result = testResponseRetriever.Retrieve(testRequest);

            Assert.True(result.Equals(testResponse));
        }

        [Fact]
        public void RetrieveTakesARequestForAPathOtherThanSimpleGetAndThenReturnsAResponseOfFourOFour()
        {
            var testRequest = new Request(
                "GET",
                "/",
                "HTTP/1.1"
            );

            var testResponse = new Response(
                "HTTP/1.1",
                404,
                "Not Found",
                new Dictionary<string, string>
                {
                    {"Date", "Tue, 02 Dec 1997 15:10:00 GMT"}
                }
            );

            var testResponseRetriever = new ResRetriever
            {
                DateTimeProvider = _mockDateTime.Object
            };

            var result = testResponseRetriever.Retrieve(testRequest);

            Assert.True(result.Equals(testResponse));
        }
    }
}
