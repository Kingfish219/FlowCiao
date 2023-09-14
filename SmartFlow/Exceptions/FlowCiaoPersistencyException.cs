using System;
using System.Runtime.Serialization;

namespace FlowCiao.Exceptions
{
    public class FlowCiaoPersistencyException : Exception
    {
        public FlowCiaoPersistencyException()
        {
        }

        public FlowCiaoPersistencyException(string message) : base(message)
        {
        }

        public FlowCiaoPersistencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FlowCiaoPersistencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
