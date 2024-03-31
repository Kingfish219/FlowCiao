using System;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handle.Handlers
{
    internal class FlowStepFinalizerHandler : FlowHandler
    {
        private readonly FlowExecutionService _executionService;

        public FlowStepFinalizerHandler(IFlowRepository flowRepository,
            FlowService flowService,
            FlowExecutionService flowExecutionService) : base(flowRepository, flowService)
        {
            _executionService = flowExecutionService;
        }

        public override FlowResult Handle(FlowStepContext flowStepContext)
        {
            try
            {
                flowStepContext.FlowExecutionStep.IsCompleted = true;
                flowStepContext.FlowExecution = FlowService.Finalize(flowStepContext.FlowExecution)
                    .GetAwaiter().GetResult();

                _executionService.Modify(flowStepContext.FlowExecution).GetAwaiter().GetResult();

                return NextHandler?.Handle(flowStepContext) ?? new FlowResult
                {
                    Status = FlowResultStatus.Completed,
                    InstanceId = flowStepContext.FlowExecution.Id
                };
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
