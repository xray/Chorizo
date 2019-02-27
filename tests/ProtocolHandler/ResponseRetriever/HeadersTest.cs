using System.Collections.Generic;
using Chorizo.ProtocolHandler.ResponseRetriever;
using Xunit;
using Xunit.Sdk;

namespace Chorizo.Tests.ProtocolHandler.ResponseRetriever
{
    public class HeadersTest
    {
        [Fact]
        public void ContainsHeaderTakesInAStringAndReturnsTrueWhenAMatchingHeaderExists()
        {
            var testHeaders = new Headers();

            var updatedHeaders = testHeaders.AddHeader("test", "header");

            Assert.True(updatedHeaders.ContainsHeader("test"));
        }

        [Fact]
        public void ContainsHeaderTakesInAStringAndReturnsFalseWhenNoMatchingHeaderExists()
        {
            var testHeaders = new Headers();

            Assert.False(testHeaders.ContainsHeader("test"));
        }

        [Fact]
        public void GetHeaderReturnsAHeaderWhenAMatchExists()
        {
            var headers = new Headers();
            var updatedHeaders = headers.AddHeader("test", "header");

            var foundHeader = updatedHeaders.GetHeader("test");

            Assert.Equal("test", foundHeader.Name());
            Assert.Equal("header", foundHeader.Value());
        }

        [Fact]
        public void GetHeaderThrowsAKeyNotFoundExceptionWhenNoMatchExists()
        {
            var headers = new Headers();

            Assert.Throws<KeyNotFoundException>(() => headers.GetHeader("test"));
        }
    }
}
