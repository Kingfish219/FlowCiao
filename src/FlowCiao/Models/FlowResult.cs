
using System;

namespace FlowCiao.Models
{
    public class FlowResult
    {
        public FlowResult(FlowResultStatus flowResultStatus = FlowResultStatus.Completed, string message = default, Guid instanceId = default)
        {
            Status = MapStatus(flowResultStatus);
            Message = message;
            InstanceId = instanceId;
        }

        public static string MapStatus(FlowResultStatus status)
        {
            return status switch
            {
                FlowResultStatus.Completed => "completed",
                FlowResultStatus.Cancelled => "cancelled",
                FlowResultStatus.Failed => "failed",
                _ => "failed"
            };
        }
        
        public string Status { get; }
        public string Message { get; }
        public Guid InstanceId { get; }
    }

    public class FlowResult<T> : FlowResult
    {
        public T Data { get; set; }
    }

    public enum FlowResultStatus
    {
        Cancelled,
        Completed,
        Failed
    }
}
