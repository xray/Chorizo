using System;
using System.Runtime.Serialization;

namespace Chorizo.Logger.Exceptions
{
    [Serializable]
    public class DestinationDoesNotExistException : Exception
    {
        public DestinationDoesNotExistException()
        {
        }

        public DestinationDoesNotExistException(string message) : base(message)
        {
        }

        public DestinationDoesNotExistException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DestinationDoesNotExistException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}