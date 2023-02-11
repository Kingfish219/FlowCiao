using SmartFlow.Core.Models;
using System;
using System.Linq;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Repositories;

namespace SmartFlow.Core.Handlers
{
    internal class ActionHandler : WorkflowHandler
    {
        public ActionHandler(IProcessRepository processRepository
            , IProcessStepService processStepManager) : base(processRepository, processStepManager)
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
                var result = ProcessRepository.CompleteProgressAction(processStepContext.ProcessStepDetail,
                    processStepContext.ProcessStepDetail.TransitionActions
                    .FirstOrDefault(x => x.Action.ActionTypeCode == processStepContext.ProcessStepInput.ActionCode)
                    .Action).Result;
                if (!result)
                {
                    throw new SmartFlowProcessExecutionException("Exception occured while completing progress action");
                }

                processStepContext.ProcessStepDetail.IsCompleted = true;

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
