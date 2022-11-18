
using System;
using System.Linq;
using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Handlers
{
    internal class GoToStateProcessStepFinalizerHandler : WorkflowHandler
    {
        private readonly IProcessStepManager _processStepManager;
        private readonly Guid _targetStatus;
        private Guid _UserId;
        private Guid _RequsetTypeId;
        private readonly IProcessRepository _processRepository;
        private int _entityType;
        public GoToStateProcessStepFinalizerHandler(IProcessRepository processRepository, IProcessStepManager processStepManager, Guid targetStatus, int EntityType, Guid UserId, Guid requsetTypeId) : base(processRepository)
        {
            _processStepManager = processStepManager;
            _targetStatus = targetStatus;
            _UserId = UserId;
            _RequsetTypeId = requsetTypeId;
            _processRepository = processRepository;
            _entityType = EntityType;
        }
        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                var result = _processStepManager.GoToStateFinalizeActiveProcessStep(processStep,_targetStatus);
                if (result.Status == ProcessResultStatus.Failed)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Failed,
                        Message = "Finalizing process step failed"
                    };
                }

                var state = ProcessRepository.GetState(_targetStatus).Result;
                if (state.IsFinalResponse)
                {
                    return new ProcessResult
                    {
                        Status = ProcessResultStatus.Completed
                    };
                }

                processStep = _processStepManager.InitializeActiveProcessStep(_UserId, _RequsetTypeId, _entityType, false);
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
