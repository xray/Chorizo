using System;
using System.Collections.Generic;

namespace Chorizo
{
    public class Request : IEquatable<Request>
    {
        public string Method { get; }
        public string Path { get; }
        public string Version { get; }
        public Dictionary<string, string> Headers { get; }
        public string Body { get; set; }

        public Request(string method, string path, string version, Dictionary<string, string> headers)
        {
            Method = method;
            Path = path;
            Version = version;
            Headers = headers;
        }

        public bool Equals(Request other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Method, other.Method) && string.Equals(Path, other.Path) && string.Equals(Version, other.Version) && Equals(Headers, other.Headers) && string.Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Request) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Method != null ? Method.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Version != null ? Version.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Headers != null ? Headers.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Body != null ? Body.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}