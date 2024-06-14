using System;
using FlowCiao.Exceptions;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services.Interfaces;

namespace FlowCiao.Handle.Handlers
{
    internal class TransitionHandler : FlowHandler
    {
        public TransitionHandler(IFlowRepository flowRepository
            , IFlowService flowService) : base(flowRepository, flowService)
        {
        }

        public override FlowResult Handle(FlowStepContext flowStepContext)
        {
            try
            {
                if (flowStepContext.FlowInstanceStepDetail is null)
                {
                    throw new FlowCiaoExecutionException("Exception occured while completing progress transition");
                }

                if (!flowStepContext.FlowInstanceStepDetail.IsCompleted)
                {
                    throw new FlowCiaoExecutionException(
                        "Exception occured while completing progress transition, flow step action is not yet completed");
                }

                var transition = flowStepContext.FlowInstanceStepDetail.Transition;
                if (transition.Condition is null)
                {
                    return NextHandler.Handle(flowStepContext);
                }

                if (!transition.Condition())
                {
                    throw new FlowCiaoExecutionException(
                        "Exception occured while completing transition as transition condition did not meet");
                }

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