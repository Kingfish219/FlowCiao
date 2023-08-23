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

        //internal TransitionHandler(IProcessRepository processRepository, IEntityRepository entityRepository, int actionCode, string connectionString, LogRepository logRepository) : base(processRepository)
        //{
        //    _processRepository = processRepository;
        //    _entityRepository = entityRepository;
        //    _actionCode = actionCode;
        //    _connectionString = connectionString;
        //    _logRepository = logRepository;
        //}

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

                //var result = _entityRepository.ChangeState(processStepContext.ProcessStep.Entity, transition.NextStateId);
                //if (result.Status != ProcessResultStatus.Completed)
                //{
                //    throw new SmartFlowProcessExecutionException("Exception occured while changing entity state");
                //}

                processStepContext.ProcessExecution.State = processStepContext.ProcessExecutionStepDetail.Transition.To;
                processStepContext.ProcessExecutionStep.IsCompleted = true;

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
