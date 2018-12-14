using Xunit;
using Chorizo;

namespace Chorizo.UnitTests.Server
{
  public class Server_PrinterShould
  {
    [Fact]
    public void ReturnTrueGivenTrue()
    {
      var result = Chorizo.Server.Printer();

      Assert.False(result, "Printer should return False");
    }
  }
}
