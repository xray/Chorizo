using System;

namespace Chorizo.Date
{
    public interface IDateTimeProvider
    {
        DateTime Now();
        string FormatDateFile(DateTime date);
        string FormatDateFull(DateTime date);
        string FormatDateISO(DateTime date);
    }
}
