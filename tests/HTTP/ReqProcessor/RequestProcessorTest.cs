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
        public void ProcessTakesAGetRequestForForwardSlashSimpleGetAndReturnsAResponseOfTwoHundredOK()
        {
            var testRequest = new Request(
                "GET",
                "/simple_get",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var testResponseRetriever = new RequestProcessor(_mockDateTime.Object);

            var result = testResponseRetriever.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void ProcessTakesARequestForAnyUndefinedPath()
        {
            var testRequest = new Request(
                "GET",
                "/",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 404, "Not Found")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var testResponseRetriever = new RequestProcessor(_mockDateTime.Object);

            var result = testResponseRetriever.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void ProcessTakesAPostRequestForForwardSlashEchoBodyAndReturnsAResponseOf200WithABodyEqualToTheBodyOfTheRequest()
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

            var result = testRequestProcessor.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void
            ProcessTakesAOptionsRequestForForwardSlashMethodOptionsAndReturnsAResponseWithAnAllowHeaderThatContainsGetHeadAndOptions()
        {
            var testRequest = new Request(
                "OPTIONS",
                "/method_options",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT")
                .AddHeader("Allow", "GET,HEAD,OPTIONS");

            var testRequestProcessor = new RequestProcessor(_mockDateTime.Object);

            var result = testRequestProcessor.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void
            ProcessTakesAOptionsRequestForForwardSlashMethodOptions2AndReturnsAResponseWithAnAllowHeaderThatContainsGetHeadAndOptions()
        {
            var testRequest = new Request(
                "OPTIONS",
                "/method_options2",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT")
                .AddHeader("Allow", "GET,HEAD,OPTIONS,PUT,POST");

            var testRequestProcessor = new RequestProcessor(_mockDateTime.Object);

            var result = testRequestProcessor.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void
            ProcessTakesAGetRequestForForwardSlashRedirectAndReturnsAResponseWithALocationHeaderThatContainsTheURIForSimpleGet()
        {
            var testRequest = new Request(
                "GET",
                "/redirect",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 301, "Moved Permanently")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT")
                .AddHeader("Location", "http://localhost:5000/simple_get");

            var testRequestProcessor = new RequestProcessor(_mockDateTime.Object);

            var result = testRequestProcessor.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void
            ProcessTakesAHeadRequestForForwardSlashGetWithBodyAndReturnsA200OkResponse()
        {
            var testRequest = new Request(
                "HEAD",
                "/get_with_body",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 200, "OK")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT");

            var testRequestProcessor = new RequestProcessor(_mockDateTime.Object);

            var result = testRequestProcessor.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }

        [Fact]
        public void
            ProcessTakesARequestForAnyOtherMethodBesidesHeadForForwardSlashGetWithBodyAndReturnsA405MethodNotAllowedResponse()
        {
            var testRequest = new Request(
                "GET",
                "/get_with_body",
                "HTTP/1.1"
            );

            var testResponse = new Response("HTTP/1.1", 405, "Method Not Allowed")
                .AddHeader("Date", "Tue, 02 Dec 1997 15:10:00 GMT")
                .AddHeader("Allow", "HEAD,OPTIONS");

            var testRequestProcessor = new RequestProcessor(_mockDateTime.Object);

            var result = testRequestProcessor.HandleRequest(testRequest);

            Assert.True(testResponse.Equals(result));
        }
    }
}
