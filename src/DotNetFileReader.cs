using System.IO;

namespace Chorizo
{
    public class DotNetFileReader : IFileReader
    {
        private StreamReader _streamReader;
        public DotNetFileReader(string path)
        {
            _streamReader = new StreamReader(path);
        }
        public string ReadToEnd()
        {
            return _streamReader.ReadToEnd();
        }

        public void Dispose()
        {
            _streamReader?.Dispose();
        }
    }
}