
namespace SmartFlow.Core.Models
{
    public class ProcessResult
    {
        public ProcessResultStatus Status { get; set; } = ProcessResultStatus.Completed;
        public string Message { get; set; }
    }

    public class ProcessResult<T>
    {
        public ProcessResultStatus Status { get; set; } = ProcessResultStatus.Completed;
        public string Message { get; set; }
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
