namespace Chorizo
{
    public class Request
    {
        public Header[] Headers;
        public string Body;
        public string Method;
        public RequestParameter[] Parameters;
        public string Path;
    }
}