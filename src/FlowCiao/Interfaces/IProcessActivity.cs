using FlowCiao.Models;

namespace FlowCiao.Interfaces
{
    public interface IProcessActivity
    {
        ProcessResult Execute(ProcessStepContext context);
    }
}