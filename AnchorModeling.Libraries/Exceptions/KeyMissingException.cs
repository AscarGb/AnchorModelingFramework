using System;
using System.Runtime.Serialization;

namespace AnchorModeling
{
    [Serializable]
    public class KeyMissingException : Exception
    {
        public KeyMissingException()
        {
        }

        public KeyMissingException(string message) : base(message)
        {
        }

        public KeyMissingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KeyMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}