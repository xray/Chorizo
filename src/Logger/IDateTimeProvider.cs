using System;

namespace Chorizo.Logger
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}