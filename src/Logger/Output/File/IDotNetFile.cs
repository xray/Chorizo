namespace Chorizo.Logger.Output.File
{
    public interface IDotNetFile
    {
        void WriteAllLines(string path, string[] contents);
        void AppendAllLines(string path, string[] contents);
    }
}