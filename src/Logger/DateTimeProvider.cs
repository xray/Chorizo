using System;

namespace Chorizo.Logger
{
    public class DateTimeProvider : IDateTimeProvider
    {        
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public string FormatDateFile(DateTime date)
        {
            return date.ToString("yyyy-MM-dd-HHmmss");
        }
        
        public string FormatDateFull(DateTime date)
        {
            return date.ToString("MMM d, yyyy @ hh:mm tt (K)");
        }

        public string FormatDateISO(DateTime date)
        {
            return date.ToString("dd/MMM/yyyy:HH:mm:ss zzz");
        }
    }
}