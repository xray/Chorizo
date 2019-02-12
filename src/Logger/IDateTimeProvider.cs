using System;

namespace Chorizo.Logger
{
    public interface IDateTimeProvider
    {
        DateTime Now();
        string FormatDateFile(DateTime date);
        string FormatDateFull(DateTime date);
        string FormatDateISO(DateTime date);
    }
}