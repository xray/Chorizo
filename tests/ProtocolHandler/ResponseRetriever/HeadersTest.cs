using System.Collections.Generic;
using Chorizo.ProtocolHandler.HttpHeaders;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.ResponseRetriever
{
    public class HeadersTest
    {
        [Fact]
        public void AddHeaderTakesInANameAndValueAndReturnsANewHeadersWithTheAdditionalHeader()
        {
            var testHeaders = new Headers();

            var updatedHeaders = testHeaders.AddHeader("test", "header");

            var addedHeader = updatedHeaders.GetHeader("test");
            Assert.Equal("test", addedHeader.Name());
            Assert.Equal("header", addedHeader.Value());
        }

        [Fact]
        public void AddHeaderTakesInANameAndValueAndReplacesTheExistingHeaderWhenANamingConflictOccurs()
        {
            var testHeaders = new Headers()
                .AddHeader("test", "header");


            var updatedHeaders = testHeaders.AddHeader("test", "NewValue");

            var changedHeader = updatedHeaders.GetHeader("test");
            Assert.Equal("test", changedHeader.Name());
            Assert.Equal("NewValue", changedHeader.Value());
        }

        [Theory]
        [InlineData("test")]
        [InlineData("Test")]
        [InlineData("TEST")]
        public void ContainsHeaderTakesInAStringAndReturnsTrueWhenAMatchingHeaderExists(string dynamicName)
        {
            var testHeaders = new Headers();

            var updatedHeaders = testHeaders.AddHeader(dynamicName, "header");

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

        [Fact]
        public void ToStringUsesTheHeadersPropertyToBuildAStringRepresentationOfHeaders()
        {
            var testHeaders = new Headers()
                .AddHeader("test1", "1")
                .AddHeader("test2", "2")
                .AddHeader("test3", "3");

            var expectedString = "test1: 1\r\n" +
                                 "test2: 2\r\n" +
                                 "test3: 3\r\n";

            Assert.Equal(expectedString, testHeaders.ToString());
        }

        [Fact]
        public void EqualsTakesInASecondaryHeadersAndReturnsTrueWhenTheyContainTheSameInformation()
        {
            var testHeaders = new Headers()
                .AddHeader("testOne", "One")
                .AddHeader("testTwo", "Two");

            var testHeadersToCompare = new Headers()
                .AddHeader("testOne", "One")
                .AddHeader("testTwo", "Two");

            Assert.True(testHeaders.Equals(testHeadersToCompare));
        }

        [Fact]
        public void EqualsTakesInASecondaryHeadersAndReturnsFalseWhenTheyContainDifferentInformation()
        {
            var testHeaders = new Headers()
                .AddHeader("testOne", "One")
                .AddHeader("testTwo", "Two");

            var testHeadersToCompare = new Headers()
                .AddHeader("headerOne", "One")
                .AddHeader("testTwo", "Two");

            Assert.False(testHeaders.Equals(testHeadersToCompare));
        }
    }
}
