using System;
using System.Runtime.Serialization;

namespace Chorizo.Logger.Exceptions
{
    [Serializable]
    public class LevelDoesNotExistException : Exception
    {
        public LevelDoesNotExistException()
        {
        }

        public LevelDoesNotExistException(string message) : base(message)
        {
        }

        public LevelDoesNotExistException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LevelDoesNotExistException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}