using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Interfaces
{
    public interface IProcessActivity
    {
        ProcessResult Execute(ProcessStepContext context);
    }
}