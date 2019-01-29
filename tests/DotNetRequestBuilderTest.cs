using System.Collections.Generic;
using Xunit;

namespace Chorizo.Tests
{
    public class DotNetRequestBuilderTest
    {
        [Fact]
        public void BuildParsesAnHttpHeaderStringIntoARequestObject()
        {
             var testBuilder = new DotNetRequestBuilder();
             
             var testGetRequestString =
                 "GET  HTTP/1.1\r\nHost: localhost:8000\r\ncache-control: no-cache,no-cache\r\nPostman-Token: 7580f83c-1d96-4996-a58e-0395fc4296c4\r\nUser-Agent: PostmanRuntime/7.4.0\r\nAccept: */*\r\nHost: localhost:8000\r\naccept-encoding: gzip, deflate\r\nConnection: keep-alive\r\n\r\n";
             var testParams = new Dictionary<string, string>
             {
                 {"cache-control", "no-cache"},
                 {"Postman-Token", "3f77f85c-b78e-46ef-94cc-a82b1cbacc86"},
                 {"User-Agent", "PostmanRuntime/7.4.0"},
                 {"Accept", "*/*"},
                 {"Host", "localhost:8000"},
                 {"accept-encoding", "gzip, deflate"},
                 {"Connection", "keep-alive"}
             };

             var expectedRequest = new Request("GET", "", "HTTP/1.1", testParams);
             
             Assert.Equal(testBuilder.Build(testGetRequestString), expectedRequest);
        }
    }
}