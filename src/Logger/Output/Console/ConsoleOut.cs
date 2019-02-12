using System;

namespace Chorizo.Logger.Output.Console
{
    public class ConsoleOut : ILoggerOut
    {
        public IConsole WrappedConsole { private get; set; }
        private const int Error = 0;
        private const int Info = 1;
        private const int Warning = 2;

        public ConsoleOut(IConsole wrappedConsole = null)
        {
            WrappedConsole = wrappedConsole ?? new Console();
        }

        public void Out(string toOutput, int logLevel, DateTime currentTime)
        {
            var formattedTime = currentTime.ToString("t");
            var tag = TagBuilder(logLevel);
            WrappedConsole.WriteLine($"{formattedTime} -> {tag} {toOutput}");
        }

        private static string TagBuilder(int logLevel)
       {
           switch (logLevel)
           {
               case Error:
                   return "[\x1b[31mERROR\x1b[0m]";
               case Info:
                   return "[\x1b[92mINFO\x1b[0m]";
               case Warning:
                   return "[\x1b[93mWARNING\x1b[0m]";
               default:
                   throw new NotSupportedException();
           }
       }
    }
}
