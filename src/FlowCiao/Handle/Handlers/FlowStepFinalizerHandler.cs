using System;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handle.Handlers
{
    internal class FlowStepFinalizerHandler : FlowHandler
    {
        private readonly FlowInstanceService _instanceService;

        public FlowStepFinalizerHandler(IFlowRepository flowRepository,
            FlowService flowService,
            FlowInstanceService flowInstanceService) : base(flowRepository, flowService)
        {
            _instanceService = flowInstanceService;
        }

        public override FlowResult Handle(FlowStepContext flowStepContext)
        {
            try
            {
                flowStepContext.FlowInstanceStep.IsCompleted = true;
                flowStepContext.FlowInstance = FlowService.Finalize(flowStepContext.FlowInstance)
                    .GetAwaiter().GetResult();

                _instanceService.Modify(flowStepContext.FlowInstance).GetAwaiter().GetResult();

                return new FlowResult(FlowResultStatus.Completed, message: "Triggering was successful",
                    instanceId: flowStepContext.FlowInstance.Id);
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