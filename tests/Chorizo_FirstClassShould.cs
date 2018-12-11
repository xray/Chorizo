using Xunit;
using Chorizo.Base;

namespace Chorizo.UnitTests.Base
{
  public class FirstClass_IsTrueShould
  {
    private readonly FirstClass _firstClass;

    public FirstClass_IsTrueShould()
    {
      _firstClass = new FirstClass();
    }

    [Fact]
    public void ReturnTrueGivenTrue()
    {
      var result = _firstClass.IsTrue(true);

      Assert.True(result, "true should return true");
    }

    [Fact]
    public void ReturnFalseGivenFalse()
    {
      var result = _firstClass.IsTrue(false);

      Assert.False(result, "false should return false");
    }
  }
}
