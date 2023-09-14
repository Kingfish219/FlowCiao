using System;
using FlowCiao.Exceptions;
using FlowCiao.Models;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handlers
{
    internal class TransitionHandler : WorkflowHandler
    {
        public TransitionHandler(IProcessRepository processRepository
            , IProcessService processService) : base(processRepository, processService)
        {
        }

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                if (processStepContext.ProcessExecutionStepDetail is null)
                {
                    throw new FlowCiaoProcessExecutionException("Exception occured while completing progress transition");
                }

                if (!processStepContext.ProcessExecutionStepDetail.IsCompleted)
                {
                    throw new FlowCiaoProcessExecutionException("Exception occured while completing progress transition, process step action is not yet completed");
                }

                var transition = processStepContext.ProcessExecutionStepDetail.Transition;
                if (transition.Condition != null)
                {
                    if (!transition.Condition())
                    {
                        throw new FlowCiaoProcessExecutionException("Exception occured while completing transition as transition condition did not meet");
                    }
                }

                processStepContext.ProcessExecution.State = processStepContext.ProcessExecutionStepDetail.Transition.To;

                return NextHandler.Handle(processStepContext);
            }
            catch (Exception exception)
            {
                return new ProcessResult
                {
                    Status = ProcessResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public override ProcessResult RollBack(ProcessStepContext processStepContext)
        {
            return PreviousHandler?.RollBack(processStepContext) ?? new ProcessResult
            {
                Status = ProcessResultStatus.Failed
            };
        }
    }
}
