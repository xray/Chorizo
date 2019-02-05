namespace Chorizo.Logger.Output.Console
{
    public class DotNetConsole : IDotNetConsole
    {
        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}