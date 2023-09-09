using System;
using SmartFlow.Exceptions;
using SmartFlow.Models;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Services;

namespace SmartFlow.Handlers
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
                    throw new SmartFlowProcessExecutionException("Exception occured while completing progress transition");
                }

                if (!processStepContext.ProcessExecutionStepDetail.IsCompleted)
                {
                    throw new SmartFlowProcessExecutionException("Exception occured while completing progress transition, process step action is not yet completed");
                }

                var transition = processStepContext.ProcessExecutionStepDetail.Transition;
                if (transition.Condition != null)
                {
                    if (!transition.Condition())
                    {
                        throw new SmartFlowProcessExecutionException("Exception occured while completing transition as transition condition did not meet");
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
