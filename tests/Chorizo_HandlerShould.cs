using System;
using Xunit;
using Moq;
using Chorizo.Handler;

namespace Chorizo.UnitTests.Handler
{
    public class Chorizo_HandlerShould
    {
        private readonly RequestHandler _handler;

        public Chorizo_HandlerShould()
        {
            _handler = new RequestHandler();
        }
        
        [Fact]
        public void NewMethod_ShouldReturnTrue()
        {
            bool result = _handler.NewMethod(true);

            Assert.True(result, "Given true should return true.");
        }
    }
}
