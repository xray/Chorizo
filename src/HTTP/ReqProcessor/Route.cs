namespace Chorizo.HTTP.ReqProcessor
{
    public readonly struct Route
    {
        public readonly string Name;
        private readonly Action _action;

        public Route(string name, Action action)
        {
            Name = name;
            _action = action;
        }
    }
}
