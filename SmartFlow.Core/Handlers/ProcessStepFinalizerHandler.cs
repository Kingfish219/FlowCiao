using System;
using SmartFlow.Core.Models;
using SmartFlow.Core.Persistence.Interfaces;

namespace SmartFlow.Core.Handlers
{
    internal class ProcessStepFinalizerHandler : WorkflowHandler
    {
        public ProcessStepFinalizerHandler(IProcessRepository processRepository
            , IProcessStepService processStepManager) : base(processRepository, processStepManager)
        {
        }

        //public ProcessStepFinalizerHandler(IProcessRepository processRepository, ProcessStepContext ProcessStepContext, IProcessStepManager processStepManager) : base(processRepository, ProcessStepContext)
        //{
        //    _processStepManager = processStepManager;
        //    _actionCode = actionCode;
        //    _userId = userId;
        //    _processRepository = processRepository;
        //    _entity = entity;
        //}

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                //var result = ProcessStepManager.FinalizeActiveProcessStep(processStepContext.ProcessStepDetail);
                //if (result.Status == ProcessResultStatus.Failed)
                //{
                //    return new ProcessResult
                //    {
                //        Status = ProcessResultStatus.Failed,
                //        Message = "Finalizing process step failed"
                //    };
                //}

                //var state = ProcessRepository.GetState(processStepContext.ProcessStepDetail.TransitionActions
                //    .FirstOrDefault(x => x.Action.ActionTypeCode == processStepContext.ProcessStepInput.ActionCode)
                //    .Transition.NextStateId).Result;
                //if (state.IsFinal)
                //{
                //    return new ProcessResult
                //    {
                //        Status = ProcessResultStatus.Completed
                //    };
                //}

                //processStepContext.ProcessStepDetail = ProcessStepManager.InitializeActiveProcessStep(processStepContext.ProcessUser.Id
                //    , processStepContext.ProcessStepDetail.Entity
                //    , false);

                //if (processStepContext.ProcessStepDetail == null)
                //{
                //    return new ProcessResult
                //    {
                //        Status = ProcessResultStatus.Failed,
                //        Message = "Initializing process step failed"
                //    };
                //}

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
