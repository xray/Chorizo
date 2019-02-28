using Chorizo.ProtocolHandler.HttpHeaders;
using Xunit;

namespace Chorizo.Tests.ProtocolHandler.ResponseRetriever
{
    public class HeaderTest
    {
        [Fact]
        public void ToStringUsesThePropertiesOfHeaderToGenerateAStringRepresentationOfTheHeader()
        {
            var testHeader = new Header("name", "value");

            Assert.Equal("name: value\r\n", testHeader.ToString());
        }

        [Fact]
        public void EqualsTakesInAHeaderAndReturnsTrueIfTheHeaderNameAndValueMatch()
        {
            var testHeader = new Header("test", "Header");
            var comparisonHeader = new Header("test", "Header");

            Assert.True(testHeader.Equals(comparisonHeader));
        }

        [Fact]
        public void EqualsTakesInAHeaderAndReturnsFalseIfTheHeadNameAndValueDoNotMatch()
        {
            var testHeader = new Header("test", "Header");
            var comparisonHeader = new Header("test", "NotHeader");

            Assert.False(testHeader.Equals(comparisonHeader));
        }
    }
}
