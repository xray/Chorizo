using System;

namespace Chorizo.Logger
{
    public class DotNetDateTime : IDotNetDateTime
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}