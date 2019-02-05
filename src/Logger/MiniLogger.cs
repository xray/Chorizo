using System;
using Chorizo.Logger.Configuration;
using Chorizo.Logger.Exceptions;
using Chorizo.Logger.Output;
using Chorizo.Logger.Output.Console;
using Chorizo.Logger.Output.File;

namespace Chorizo.Logger
{
    public class MiniLogger : IMiniLogger
    {
        private readonly ILoggerOut _userInterfaceOut;
        private readonly ILoggerOut _flatFileOut;
        private readonly IDotNetDateTime _dateTime;
        private readonly int _level;
        private readonly int _destination;
        
        private const int UserInterface = 0;
        private const int File = 1;
        private const int Both = 2;
        
        public MiniLogger(
            LogConfig logConfig,
            ILoggerOut userOut = null, 
            ILoggerOut fileOut = null,
            IDotNetDateTime dateTime = null
            )
        {
            _dateTime = dateTime ?? new DotNetDateTime();
            _userInterfaceOut = userOut?? new ConsoleOut();
            _flatFileOut = fileOut ?? new FileOut(_dateTime.Now());
            _level = LevelConvert(logConfig.Level);
            _destination = DestinationConvert(logConfig.Destination);
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
            switch (input)
            {
                case "test":
                    return 0;
                case "prod":
                    return 1;
                case "dev":
                    return 2;
                default:
                    throw new LevelDoesNotExistException(
                        $"{input} is not a valid level for MiniLogger. " +
                        "The valid levels are \"prod\", \"dev\", and \"test\"."
                    );
            }
        }
        
        private int DestinationConvert(string input)
        {
            switch (input)
            {
                case "ui":
                    return UserInterface;
                case "file":
                    return File;
                case "both":
                    return Both;
                default:
                    throw new DestinationDoesNotExistException(
                        $"{input} is not a valid destination for MiniLogger. " +
                        "The valid destinations are \"ui\", \"file\", and \"both\"."
                    );
            }
        }

        private void Dispatch(Tuple<int, string> toLog)
        {
            var (logLevel, message) = toLog;

            if (logLevel > _level) return;
            switch (_destination)
            {
                case UserInterface:
                    _userInterfaceOut.Out(message, logLevel, _dateTime.Now());
                    break;
                case File:
                    _flatFileOut.Out(message, logLevel, _dateTime.Now());
                    break;
                case Both:
                    _userInterfaceOut.Out(message, logLevel, _dateTime.Now());
                    _flatFileOut.Out(message, logLevel, _dateTime.Now());
                    break;
            }
        }
    }
}
