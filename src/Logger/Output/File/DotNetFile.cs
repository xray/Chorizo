namespace Chorizo.Logger.Output.File
{
    public class DotNetFile : IDotNetFile
    {
        public void WriteAllLines(string path, string[] contents)
        {
            System.IO.File.WriteAllLines(path, contents);
        }

        public void AppendAllLines(string path, string[] contents)
        {
            System.IO.File.AppendAllLines(path, contents);
        }
    }
}