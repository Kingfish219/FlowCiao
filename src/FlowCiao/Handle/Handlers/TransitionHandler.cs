using System;
using FlowCiao.Exceptions;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

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
                if (flowStepContext.FlowExecutionStepDetail is null)
                {
                    throw new FlowExecutionException("Exception occured while completing progress transition");
                }

                if (!flowStepContext.FlowExecutionStepDetail.IsCompleted)
                {
                    throw new FlowExecutionException("Exception occured while completing progress transition, flow step action is not yet completed");
                }

                var transition = flowStepContext.FlowExecutionStepDetail.Transition;
                if (transition.Condition != null)
                {
                    if (!transition.Condition())
                    {
                        throw new FlowExecutionException("Exception occured while completing transition as transition condition did not meet");
                    }
                }

                flowStepContext.FlowExecution.State = flowStepContext.FlowExecutionStepDetail.Transition.To;

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
