using System;

namespace Chorizo.Logger.Output
{
    public interface ILoggerOut
    {
        void Out(string toOutput, int logLevel, DateTime currentTime);
    }
}