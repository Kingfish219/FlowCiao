using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;

namespace SmartFlow.Core.Handlers
{
    internal class TransitionHandler : WorkflowHandler
    {
        private readonly IEntityRepository _entityRepository;

        public TransitionHandler(IProcessRepository processRepository
            , IProcessStepManager processStepManager
            , ProcessStepContext processStepContext
            , IEntityRepository entityRepository) : base(processRepository, processStepManager, processStepContext)
        {
            _entityRepository = entityRepository;
        }

        //internal TransitionHandler(IProcessRepository processRepository, IEntityRepository entityRepository, int actionCode, string connectionString, LogRepository logRepository) : base(processRepository)
        //{
        //    _processRepository = processRepository;
        //    _entityRepository = entityRepository;
        //    _actionCode = actionCode;
        //    _connectionString = connectionString;
        //    _logRepository = logRepository;
        //}

        public override ProcessResult Handle()
        {
            try
            {
                var transition = ProcessStepContext.ProcessStep.TransitionActions.FirstOrDefault(x => x.Action.ActionTypeCode == ProcessStepContext.ProcessStepInput.ActionCode).Transition;
                if (transition is null)
                {
                    throw new SmartFlowProcessException("Exception occured while completing progress transition");
                }

                if (!ProcessStepContext.ProcessStep.IsCompleted)
                {
                    throw new SmartFlowProcessException("Exception occured while completing progress transition, process step action is not yet completed");
                }

                var result = _entityRepository.ChangeState(ProcessStepContext.ProcessStep.Entity, transition.NextStateId);
                if (result.Status != ProcessResultStatus.Completed)
                {
                    throw new SmartFlowProcessException("Exception occured while changing entity state");
                }

                ProcessStepContext.ProcessStep.Entity.LastStatus = transition.NextStateId;

                return NextHandler.Handle();
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

        public override ProcessResult RollBack()
        {
            throw new NotImplementedException();
        }
    }
}
