using System;
using System.Runtime.Serialization;

namespace AnchorModeling.Models
{
    [Serializable]
    internal class StateNotSupportsException : Exception
    {
        public StateNotSupportsException()
        {
        }

        public StateNotSupportsException(string message) : base(message)
        {
        }

        public StateNotSupportsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StateNotSupportsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}