namespace Chorizo.Logger.Output.Console
{
    public class Console : IConsole
    {
        public void WriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}