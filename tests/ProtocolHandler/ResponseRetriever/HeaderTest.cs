using Chorizo.ProtocolHandler.ResponseRetriever;
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
    }
}
