using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Interfaces
{
    public interface IFlowActivity
    {
        void Execute(FlowStepContext context);
    }
}