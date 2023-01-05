using System;
using System.Runtime.Serialization;

namespace SmartFlow.Core.Exceptions
{
    public class SmartFlowPersistencyException : Exception
    {
        public SmartFlowPersistencyException()
        {
        }

        public SmartFlowPersistencyException(string message) : base(message)
        {
        }

        public SmartFlowPersistencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SmartFlowPersistencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
