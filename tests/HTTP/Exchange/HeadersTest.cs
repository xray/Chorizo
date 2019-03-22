using System.Collections.Generic;
using Chorizo.HTTP.Exchange;
using Xunit;

namespace Chorizo.Tests.HTTP.Exchange
{
    public class HeadersTest
    {
        [Fact]
        public void AddHeaderTakesInANameAndValueAndReturnsANewHeadersWithTheAdditionalHeader()
        {
            var testHeaders = new Headers("test", "header");

            var updatedHeaders = testHeaders.AddHeader("additional", "header");

            var addedHeader = updatedHeaders["additional"];
            Assert.Equal("additional", addedHeader.Name);
            Assert.Equal("header", addedHeader.Value);
        }

        [Fact]
        public void AddHeaderTakesInANameAndValueAndReplacesTheExistingHeaderWhenANamingConflictOccurs()
        {
            var testHeaders = new Headers("test", "header");

            var updatedHeaders = testHeaders.AddHeader("test", "NewValue");

            var changedHeader = updatedHeaders["test"];
            Assert.Equal("test", changedHeader.Name);
            Assert.Equal("NewValue", changedHeader.Value);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("Test")]
        [InlineData("TEST")]
        public void ContainsHeaderTakesInAStringAndReturnsTrueWhenAMatchingHeaderExists(string dynamicName)
        {
            var testHeaders = new Headers(dynamicName, "header");

            Assert.True(testHeaders.ContainsHeader("test"));
        }

        [Fact]
        public void ContainsHeaderTakesInAStringAndReturnsFalseWhenNoMatchingHeaderExists()
        {
            var testHeaders = new Headers("hello", "world");

            Assert.False(testHeaders.ContainsHeader("test"));
        }

        [Fact]
        public void IndexingAHeadersObjectWithANameStringWillReturnAHeaderWhenAMatchExists()
        {
            var headers = new Headers("test", "header");

            var foundHeader = headers["test"];

            Assert.Equal("test", foundHeader.Name);
            Assert.Equal("header", foundHeader.Value);
        }

        [Fact]
        public void IndexingAHeadersObjectWithANameStringWillThrowAKeyNotFoundExceptionWhenNoMatchExists()
        {
            var headers = new Headers("hello", "world");

            Assert.Throws<KeyNotFoundException>(() => headers["test"]);
        }

        [Fact]
        public void ToStringUsesTheHeadersPropertyToBuildAStringRepresentationOfHeaders()
        {
            var testHeaders = new Headers("test1", "1")
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
            var testHeaders = new Headers("testOne", "One")
                .AddHeader("testTwo", "Two");

            var testHeadersToCompare = new Headers("testOne", "One")
                .AddHeader("testTwo", "Two");

            Assert.True(testHeaders.Equals(testHeadersToCompare));
        }

        [Fact]
        public void EqualsTakesInASecondaryHeadersAndReturnsFalseWhenTheyContainDifferentInformation()
        {
            var testHeaders = new Headers("testOne", "One")
                .AddHeader("testTwo", "Two");

            var testHeadersToCompare = new Headers("headerOne", "One")
                .AddHeader("testTwo", "Two");

            Assert.False(testHeaders.Equals(testHeadersToCompare));
        }
    }
}
