using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Interfaces
{
    public interface IFlowActivity
    {
        FlowResult Execute(FlowStepContext context);
    }
}