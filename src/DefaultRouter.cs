using System.Collections.Generic;

namespace Chorizo
{
    public class DefaultRouter:IRouter
    {
        public List<Route> Routes;
        public IMatcher Matcher { get; set; }

        public DefaultRouter()
        {
            Routes = new List<Route>();
        }
        public Response Match(Request testRequest)
        {
            var response = Matcher.Match(testRequest);
            return response;
        }

        // ASK KB:
        // How do I extract the Route.Action type out of Route so
        // DefaultRouter is not relying on Route?
        public void Get(string path, Route.Action action)
        {
            Routes.Add(new Route("GET", path, action));
        }
    }
}