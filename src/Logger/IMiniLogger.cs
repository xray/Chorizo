namespace Chorizo.Logger
{
    public interface IMiniLogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
