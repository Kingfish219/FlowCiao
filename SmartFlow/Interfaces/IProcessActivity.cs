using SmartFlow.Models;

namespace SmartFlow.Interfaces
{
    public interface IProcessActivity
    {
        ProcessResult Execute(ProcessStepContext context);
        ProcessResult Execute();
    }
}