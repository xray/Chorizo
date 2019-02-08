using System;

namespace Chorizo.Logger.Output.File
{
    public class FileOut : ILoggerOut
    {
        private readonly IDotNetFile _wrappedFile;
        private readonly DateTime _creationDateTime;
        private readonly string _dirPath;
        private readonly string _filePath;
        private bool _fileHasNotBeenInitialized = true;
        private readonly string _logName;
        private const int Error = 0;
        private const int Info = 1;
        private const int Warning = 2;

        public FileOut(
            DateTime creationDateTime,
            string directoryPath = null,
            string fileName = null,
            IDotNetFile wrappedFile = null
        )
        {
            _creationDateTime = creationDateTime;
            _wrappedFile = wrappedFile ?? new DotNetFile();
            _logName = fileName ?? "Chorizo";
            _dirPath = directoryPath ?? @"logs/";
            var fileNameTime = _creationDateTime.ToString("yyyy-MM-dd-HHmmss");
            
            _filePath = $"{_dirPath}{fileNameTime}_{_logName}.md";
        }

        public void Out(string toOutput, int logLevel, DateTime currentTime)
        {
            if (_fileHasNotBeenInitialized)
            {
                InitializeFile();
            }
            var formattedDate = currentTime.ToString("dd/MMM/yyyy:HH:mm:ss zzz");
            var logTypeLine = $"## {LogTypeIdentifier(logLevel)}  ";
            var dateLine = $"**Time**: {formattedDate}";
            var messageLine = $"> {toOutput}";
            const string dividerLine = "---";
            var lines = new[] {logTypeLine, dateLine, messageLine, dividerLine};
            _wrappedFile.AppendAllLines(_filePath, lines);
        }

        private void InitializeFile()
        {
            var formattedDate = _creationDateTime.ToString("MMM d, yyyy @ hh:mm tt (K)");
            var nameLine = $"# {_logName}  ";
            var dateLine = $"#### Initialized On {formattedDate}  ";
            var initialLines = new [] {nameLine, dateLine, "---", "---", "" };
            _wrappedFile.CreateDirectory(_dirPath);
            _wrappedFile.WriteAllLines(_filePath, initialLines);
            _fileHasNotBeenInitialized = false;
        }
        
        private static string LogTypeIdentifier(int logLevel)
        {
            switch (logLevel)
            {
                case Error:
                    return "Error";
                case Info:
                    return "Info";
                case Warning:
                    return "Warning";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
