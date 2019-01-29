using System;

namespace Chorizo
{
    public class Response : IEquatable<Response>
    {
        public bool isResponse;

        public bool Equals(Response other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return isResponse == other.isResponse;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Response) obj);
        }

        public override int GetHashCode()
        {
            return isResponse.GetHashCode();
        }
    }
}