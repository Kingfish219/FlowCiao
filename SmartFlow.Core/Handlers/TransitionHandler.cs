using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Persistence.Interfaces;

namespace SmartFlow.Core.Handlers
{
    internal class TransitionHandler : WorkflowHandler
    {
        private readonly IEntityRepository _entityRepository;

        public TransitionHandler(IProcessRepository processRepository
            , IProcessStepService processStepManager) : base(processRepository, processStepManager)
        {
            //_entityRepository = entityRepository;
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
                var transition = processStepContext.ProcessStep.Details
                    .FirstOrDefault(x => x.Transition.Actions
                        .FirstOrDefault(y => y.Code == processStepContext.ProcessStepInput.ActionCode)).Transition;
                if (transition is null)
                {
                    throw new SmartFlowProcessExecutionException("Exception occured while completing progress transition");
                }

                if (!processStepContext.ProcessStepDetail.IsCompleted)
                {
                    throw new SmartFlowProcessExecutionException("Exception occured while completing progress transition, process step action is not yet completed");
                }

                var result = _entityRepository.ChangeState(processStepContext.ProcessStep.Entity, transition.NextStateId);
                if (result.Status != ProcessResultStatus.Completed)
                {
                    throw new SmartFlowProcessExecutionException("Exception occured while changing entity state");
                }

                processStepContext.ProcessStep.Entity.LastStatus = transition.NextStateId;

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
