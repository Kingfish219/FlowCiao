using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    public interface IProcessActivity
    {
        ProcessResult Execute(ProcessStepContext context);
        ProcessResult Execute();
    }
}