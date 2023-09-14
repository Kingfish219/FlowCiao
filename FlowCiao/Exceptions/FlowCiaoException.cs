using System;
using System.Runtime.Serialization;

namespace FlowCiao.Exceptions
{
    public class FlowCiaoException : Exception
    {
        public FlowCiaoException()
        {
        }

        public FlowCiaoException(string message) : base(message)
        {
        }

        public FlowCiaoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FlowCiaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
