using System;
using System.Collections.Generic;

namespace Chorizo
{
    public class DefaultRouter:IRouter
    {
        public delegate void Action(Request req, Response res);
        
        public List<Route> Routes;
        public IMatcher Matcher { get; set; }

        public DefaultRouter()
        {
            Routes = new List<Route>();
        }
        
        // Note From Avni:
        // Consider adding a way to batch configure routes by passing in a file.
        
        // ASK KB:
        // Should I just put the logic for the matching Requests to
        // Routes in the Match function or should I leave it as is
        // with the functionality abstracted away?
        public bool Match(string requestMethod, string uri)
        {
            var matched = false;
            foreach (var route in Routes)
            {
                if (route.HttpMethod == requestMethod && route.RoutePath == uri)
                {
                    matched = true;
                }
            }
            return matched;
        }
        
        // ASK KB:
        // How do I extract the Route.Action type out of Route so
        // DefaultRouter is not relying on Route?
        public void Get(string path, Action action)
        {
            Routes.Add(new Route("GET", path, action));
        }
    }
}