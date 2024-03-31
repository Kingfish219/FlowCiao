
using System;

namespace FlowCiao.Models
{
    public class FlowResult
    {
        public FlowResultStatus Status { get; set; } = FlowResultStatus.Completed;
        public string Message { get; set; }
        public Guid InstanceId { get; set; }

        public static FlowResult Success()
        {
            return new FlowResult
            {
                Status = FlowResultStatus.Completed,
                Message = "Success"
            };
        }
    }

    public class FlowResult<T> : FlowResult
    {
        public T Data { get; set; }
    }

    public enum FlowResultStatus
    {
        NotStarted,
        Cancelled,
        Completed,
        Failed,
        SetOwner
    }
}
