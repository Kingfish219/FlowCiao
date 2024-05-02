using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Interfaces
{
    internal interface IFlowHandler
    {
        void SetNextHandler(IFlowHandler handler);
        void SetPreviousHandler(IFlowHandler handler);
        FlowResult Handle(FlowStepContext flowStepContext);
        FlowResult RollBack(FlowStepContext flowStepContext);
    }
}
