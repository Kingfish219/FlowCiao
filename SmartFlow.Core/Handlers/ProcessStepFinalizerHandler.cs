using System;
using System.Linq;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Handlers
{
    internal class ProcessStepFinalizerHandler : WorkflowHandler
    {
        public ProcessStepFinalizerHandler(IProcessRepository processRepository
            , IProcessStepManager processStepManager
            , ProcessStepContext processStepContext) : base(processRepository, processStepManager, processStepContext)
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

        public override ProcessResult Handle()
        {
            try
            {
                var result = ProcessStepManager.FinalizeActiveProcessStep(ProcessStepContext.ProcessStep);
                if (result.Status == ProcessResultStatus.Failed)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Failed,
                        Message = "Finalizing process step failed"
                    };
                }

                var state = ProcessRepository.GetState(ProcessStepContext.ProcessStep.TransitionActions
                    .FirstOrDefault(x => x.Action.ActionTypeCode == ProcessStepContext.ProcessStepInput.ActionCode)
                    .Transition.NextStateId).Result;
                if (state.IsFinalResponse)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Completed
                    };
                }

                ProcessStepContext.ProcessStep = ProcessStepManager.InitializeActiveProcessStep(ProcessStepContext.ProcessUser.Id
                    , ProcessStepContext.ProcessStep.Entity
                    , false);

                if (ProcessStepContext.ProcessStep == null)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Failed,
                        Message = "Initializing process step failed"
                    };
                }

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
