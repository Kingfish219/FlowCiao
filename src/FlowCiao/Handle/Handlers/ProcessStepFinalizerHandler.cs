using System;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handle.Handlers
{
    internal class ProcessStepFinalizerHandler : WorkflowHandler
    {
        private readonly ProcessExecutionService _executionService;

        public ProcessStepFinalizerHandler(IProcessRepository processRepository,
            IProcessService processService,
            ProcessExecutionService processExecutionService) : base(processRepository, processService)
        {
            _executionService = processExecutionService;
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
                processStepContext.ProcessExecutionStep.IsCompleted = true;
                processStepContext.ProcessExecution = ProcessService.Finalize(processStepContext.ProcessExecution)
                    .GetAwaiter().GetResult();
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
                //    .FirstOrDefault(x => x.Action.TriggerType == processStepContext.ProcessStepInput.TriggerCode)
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

                _executionService.Modify(processStepContext.ProcessExecution).GetAwaiter().GetResult();

                return NextHandler?.Handle(processStepContext) ?? new ProcessResult
                {
                    Status = ProcessResultStatus.Completed,
                    InstanceId = processStepContext.ProcessExecution.Id
                };
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
