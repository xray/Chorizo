namespace Chorizo.Logger.Configuration
{
    public class LogConfig
    {
        public readonly string Level;
        public readonly string Destination;
        
        public LogConfig(string level, string destination)
        {
            Level = level;
            Destination = destination;
        }
    }
}