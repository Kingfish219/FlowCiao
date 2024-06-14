using System;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Execution;

namespace FlowCiao.Handle.Handlers
{
    internal class TriggerActivityHandler : FlowHandler
    {
        public TriggerActivityHandler(IFlowRepository flowRepository
            , IFlowService flowService) : base(flowRepository, flowService)
        {
        }

        public override FlowResult Handle(FlowStepContext flowStepContext)
        {
            try
            {
                return NextHandler.Handle(flowStepContext);
            }
            catch (Exception exception)
            {
                return new FlowResult(FlowResultStatus.Failed, message: exception.Message);
            }
        }

        public override FlowResult RollBack(FlowStepContext flowStepContext)
        {
            return PreviousHandler?.RollBack(flowStepContext) ?? new FlowResult(FlowResultStatus.Failed);
        }
    }
}
