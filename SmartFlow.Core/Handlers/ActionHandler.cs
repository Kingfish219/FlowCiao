using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Repositories;

namespace SmartFlow.Core.Handlers
{
    internal class ActionHandler : WorkflowHandler
    {
        private readonly int _actionCode;
        private readonly LogRepository _logRepository;

        internal ActionHandler(IProcessRepository processRepository, int actionCode, LogRepository logRepository) : base(processRepository)
        {
            _actionCode = actionCode;
            _logRepository = logRepository;
        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                var result = ProcessRepository.CompleteProgressAction(processStep,
                    processStep.TransitionActions.FirstOrDefault(x => x.Action.ActionTypeCode == _actionCode).Action).Result;
                if (!result)
                {
                    throw new SmartFlowProcessException("Exception occured while completing progress action", _logRepository, processStep);
                }

                processStep.IsCompleted = true;

                return NextHandler.Handle(processStep, user, processStepContext);
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

        public override ProcessResult RollBack(ProcessStep processStep)
        {
            throw new NotImplementedException();
        }
    }
}
