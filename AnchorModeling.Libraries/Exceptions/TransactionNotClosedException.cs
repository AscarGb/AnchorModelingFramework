using System;
using System.Runtime.Serialization;

namespace AnchorModeling.Models
{
    [Serializable]
    internal class TransactionNotClosedException : Exception
    {
        public TransactionNotClosedException()
        {
        }

        public TransactionNotClosedException(string message) : base(message)
        {
        }

        public TransactionNotClosedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TransactionNotClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}