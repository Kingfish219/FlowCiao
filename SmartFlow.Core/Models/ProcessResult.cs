
using System;

namespace SmartFlow.Core.Models
{
    public class ProcessResult
    {
        public ProcessResultStatus Status { get; set; } = ProcessResultStatus.Completed;
        public string Message { get; set; }
        public Guid InstanceId { get; set; }

        public static ProcessResult SuccessResult()
        {
            return new ProcessResult
            {
                Status = ProcessResultStatus.Completed,
                Message = "Success"
            };
        }
    }

    public class ProcessResult<T> : ProcessResult
    {
        public T Data { get; set; }
    }

    public enum ProcessResultStatus
    {
        NotStarted,
        Cancelled,
        Completed,
        Failed,
        SetOwner
    }
}
