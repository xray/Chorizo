using System;
using Chorizo.HTTP.Exchange;
using Chorizo.HTTP.ReqProcessor;
using Chorizo.Logger;
using Moq;
using Xunit;

namespace Chorizo.Tests.HTTP.ReqProcessor
{
    public class RequestProcessorTest
    {
        private readonly Mock<IDateTimeProvider> _mockDateTime;
        public RequestProcessorTest()
        {
            _mockDateTime = new Mock<IDateTimeProvider>();
            var testTime = new DateTime(1997, 12, 02, 15, 10, 00, DateTimeKind.Utc);
            _mockDateTime.Setup(dt => dt.Now()).Returns(testTime);
        }

        [Fact]
        public void ProcessTakesARequestForForwardSlashSimpleGetAndReturnsAResponseOfTwoHundredOK()
        {
            var testRequest = new Request(
                "GET",
                "/simple_get",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var testResponseRetriever = new RequestProcessor(_mockDateTime.Object);

            var result = testResponseRetriever.Process(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void ProcessTakesARequestForAPathOtherThanSimpleGetAndThenReturnsAResponseOfFourOFour()
        {
            var testRequest = new Request(
                "GET",
                "/",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 404, "Not Found")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var testResponseRetriever = new RequestProcessor(_mockDateTime.Object);

            var result = testResponseRetriever.Process(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void ProcessTakesARequestForForwardSlashEchoBodyAndReturnsAResponseOf200WithABodyEqualToTheBodyOfTheRequest()
        {
            var testRequest = new Request(
                "POST",
                "/echo_body",
                "HTTP/1.1",
                "some body"
            );

            var testResponse = new Response("HTTP/1.1", 200, "OK", "some body")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var testRequestProcessor = new RequestProcessor(_mockDateTime.Object);

            var result = testRequestProcessor.Process(testRequest);

            Assert.True(testResponse.Equals(result));
        }
    }
}
