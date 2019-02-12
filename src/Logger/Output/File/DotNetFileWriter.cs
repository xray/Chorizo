namespace Chorizo.Logger.Output.File
{
    public class DotNetFileWriter : IFileWriter
    {
        public void WriteAllLines(string path, string[] contents)
        {
            System.IO.File.WriteAllLines(path, contents);
        }

        public void AppendAllLines(string path, string[] contents)
        {
            System.IO.File.AppendAllLines(path, contents);
        }

        public void CreateDirectory(string path)
        {
            var file = new System.IO.FileInfo(path);
            file.Directory.Create();
        }
    }
}