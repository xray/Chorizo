using System;

namespace Chorizo
{
    public class Request : IEquatable<Request>
    {
        public bool isRequest;

        public bool Equals(Request other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return isRequest == other.isRequest;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Request) obj);
        }

        public override int GetHashCode()
        {
            return isRequest.GetHashCode();
        }
    }
}