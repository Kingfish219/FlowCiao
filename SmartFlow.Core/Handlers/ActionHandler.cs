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
            , IProcessStepManager processStepManager) : base(processRepository, processStepManager)
        {
        }

        //internal ActionHandler(IProcessRepository processRepository, ProcessStepContext processStepContext) : base(processRepository, processStepContext)
        //{
        //    _actionCode = actionCode;
        //    _logRepository = logRepository;
        //}

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                var result = ProcessRepository.CompleteProgressAction(processStepContext.ProcessStep,
                    processStepContext.ProcessStep.TransitionActions
                    .FirstOrDefault(x => x.Action.ActionTypeCode == processStepContext.ProcessStepInput.ActionCode)
                    .Action).Result;
                if (!result)
                {
                    throw new SmartFlowProcessException("Exception occured while completing progress action");
                }

                processStepContext.ProcessStep.IsCompleted = true;

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
