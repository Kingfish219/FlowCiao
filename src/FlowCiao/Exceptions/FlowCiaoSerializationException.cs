using System;
using System.Runtime.Serialization;

namespace FlowCiao.Exceptions
{
    public class FlowCiaoSerializationException : Exception
    {
        public FlowCiaoSerializationException()
        {
        }

        public FlowCiaoSerializationException(string message) : base(message)
        {
        }

        public FlowCiaoSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FlowCiaoSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
