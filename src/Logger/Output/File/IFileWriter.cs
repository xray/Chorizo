namespace Chorizo.Logger.Output.File
{
    public interface IFileWriter
    {
        void WriteAllLines(string path, string[] contents);
        void AppendAllLines(string path, string[] contents);
        void CreateDirectory(string path);
    }
}
