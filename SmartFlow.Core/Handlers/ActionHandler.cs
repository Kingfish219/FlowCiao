using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;

namespace SmartFlow.Core.Handlers
{
    internal class ActionHandler : WorkflowHandler
    {
        public ActionHandler(IProcessRepository processRepository
            , IProcessStepManager processStepManager
            , ProcessStepContext processStepContext) : base(processRepository, processStepManager, processStepContext)
        {
        }

        //internal ActionHandler(IProcessRepository processRepository, ProcessStepContext processStepContext) : base(processRepository, processStepContext)
        //{
        //    _actionCode = actionCode;
        //    _logRepository = logRepository;
        //}

        public override ProcessResult Handle()
        {
            try
            {
                var result = ProcessRepository.CompleteProgressAction(ProcessStepContext.ProcessStep,
                    ProcessStepContext.ProcessStep.TransitionActions
                    .FirstOrDefault(x => x.Action.ActionTypeCode == ProcessStepContext.ProcessStepInput.ActionCode)
                    .Action).Result;
                if (!result)
                {
                    throw new SmartFlowProcessException("Exception occured while completing progress action");
                }

                ProcessStepContext.ProcessStep.IsCompleted = true;

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
