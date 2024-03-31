using System;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

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
                return new FlowResult
                {
                    Status = FlowResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public override FlowResult RollBack(FlowStepContext flowStepContext)
        {
            return PreviousHandler?.RollBack(flowStepContext) ?? new FlowResult
            {
                Status = FlowResultStatus.Failed
            };
        }
    }
}
