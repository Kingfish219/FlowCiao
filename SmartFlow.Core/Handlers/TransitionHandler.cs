using SmartFlow.Core.Models;
using System;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Persistence.Interfaces;

namespace SmartFlow.Core.Handlers
{
    internal class TransitionHandler : WorkflowHandler
    {
        public TransitionHandler(IProcessRepository processRepository
            , IProcessStepService processStepManager) : base(processRepository, processStepManager)
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
                if (processStepContext.ProcessStepDetail is null)
                {
                    throw new SmartFlowProcessExecutionException("Exception occured while completing progress transition");
                }

                if (!processStepContext.ProcessStepDetail.IsCompleted)
                {
                    throw new SmartFlowProcessExecutionException("Exception occured while completing progress transition, process step action is not yet completed");
                }

                //var result = _entityRepository.ChangeState(processStepContext.ProcessStep.Entity, transition.NextStateId);
                //if (result.Status != ProcessResultStatus.Completed)
                //{
                //    throw new SmartFlowProcessExecutionException("Exception occured while changing entity state");
                //}

                processStepContext.Process.State = processStepContext.ProcessStepDetail.Transition.To;

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
