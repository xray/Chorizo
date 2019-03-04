using System;
using Chorizo.Date;
using Chorizo.Logger.Configuration;
using Chorizo.Logger.Output;
using Chorizo.Logger.Output.Console;
using Chorizo.Logger.Output.File;

namespace Chorizo.Logger
{
    public class MiniLogger : IMiniLogger
    {
        private readonly ILoggerOut[] _loggables;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly int _level;
        
        private enum Levels
        { test, prod, dev }
        
        public MiniLogger(
            LogConfig logConfig,
            ILoggerOut[] loggables = null,
            IDateTimeProvider dateTimeProvider = null
            )
        {
            _dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();
            _loggables = loggables ?? new ILoggerOut[]{new ConsoleOut(), new FileOut(_dateTimeProvider.Now())};
            _level = LevelConvert(logConfig.Level);
        }
        
        public void Error(string message)
        {
            Dispatch(new Tuple<int, string>(0, message));
        }

        public void Info(string message)
        {
            Dispatch(new Tuple<int, string>(1, message));
        }

        public void Warning(string message)
        {
            Dispatch(new Tuple<int, string>(2, message));
        }

        private int LevelConvert(string input)
        {
            return (int)(Levels) Enum.Parse(typeof(Levels), input);
        }
       
        private void Dispatch(Tuple<int, string> toLog)
        {
            var (logLevel, message) = toLog;
            foreach (var loggable in _loggables)
            {
                if (logLevel > _level) return;
                loggable.Out(message, logLevel, _dateTimeProvider.Now());
            }
        }
    }
}
