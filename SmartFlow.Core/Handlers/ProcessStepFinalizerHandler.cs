using System;
using System.Linq;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Handlers
{
    internal class ProcessStepFinalizerHandler : WorkflowHandler
    {
        private readonly IProcessStepManager _processStepManager;
        private readonly int _actionCode;
        private Guid _userId;
        private readonly IProcessRepository _processRepository;
        private Entity _entity;

        public ProcessStepFinalizerHandler(IProcessRepository processRepository, IProcessStepManager processStepManager, int actionCode, Entity entity, Guid userId) : base(processRepository)
        {
            _processStepManager = processStepManager;
            _actionCode = actionCode;
            _userId = userId;
            _processRepository = processRepository;
            _entity = entity;
        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                var result = _processStepManager.FinalizeActiveProcessStep(processStep);
                if (result.Status == ProcessResultStatus.Failed)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Failed,
                        Message = "Finalizing process step failed"
                    };
                }

                var state = ProcessRepository.GetState(processStep.TransitionActions.FirstOrDefault(x => x.Action.ActionTypeCode == _actionCode).Transition.NextStateId).Result;
                if (state.IsFinalResponse)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Completed
                    };
                }

                processStep = _processStepManager.InitializeActiveProcessStep(_userId, _entity, false);
                if (processStep == null)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Failed,
                        Message = "Initializing process step failed"
                    };
                }

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
