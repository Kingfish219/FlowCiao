using System;
using FlowCiao.Models;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handlers
{
    internal class ActionHandler : WorkflowHandler
    {
        public ActionHandler(IProcessRepository processRepository
            , IProcessService processService) : base(processRepository, processService)
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
                //var result = ProcessRepository.CompleteProgressAction(processStepContext.ProcessStepDetail,
                //    processStepContext.ProcessStepDetail.TransitionActions
                //    .FirstOrDefault(x => x.Action.ActionTypeCode == processStepContext.ProcessStepInput.ActionCode)
                //    .Action).Result;
                //if (!result)
                //{
                //    throw new SmartFlowProcessExecutionException("Exception occured while completing progress action");
                //}

                processStepContext.ProcessExecutionStepDetail.IsCompleted = true;

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
