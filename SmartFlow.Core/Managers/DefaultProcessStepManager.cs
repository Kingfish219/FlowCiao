using System;
using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    internal class DefaultProcessStepManager : IProcessStepManager
    {
        private readonly Entity _entity;
        private readonly IProcessRepository _processRepository;
        private readonly IEntityCreateHistory _entityCreateHistory;
        internal DefaultProcessStepManager(IProcessRepository processRepository, IEntityCreateHistory entityCreateHistory, Entity entity)
        {
            _entity = entity;
            _processRepository = processRepository;
            _entityCreateHistory = entityCreateHistory;
        }

        public ProcessStep GetActiveProcessStep(Guid userId)
        {
            var processStep = _processRepository.GetActiveProcessStep(_entity).Result;
            if (processStep != null)
            {
                var process = _processRepository.GetProcess(processStep.ProcessId).Result;
                var transitionActions = _processRepository.GetActiveTransitions(_entity, process.Id).Result;
                processStep.Entity = _entity;
                processStep.TransitionActions = transitionActions;
                processStep.Process = process;
            }

            return processStep;
        }

        //public ProcessStep InitializeActiveProcessStep(Guid userID)
        public ProcessStep InitializeActiveProcessStep(Guid userId, bool initializeFromFirstState = false)
        {
            var process = _processRepository.GetProcess(userId).Result;
            State state;
            if (initializeFromFirstState)
            {
                state = _processRepository.GetStartState();
            }
            else
            {
                state = new State { Id = _entity.LastStatus };
            }

            var transitionActions = _processRepository.GetStateTransitions(process, state).Result;
            var processStep = new ProcessStep
            {
                Process = process,
                TransitionActions = transitionActions,
                Entity = _entity
            };

            var result = _processRepository.CreateProcessStep(processStep).Result;
            if (!result)
            {
                throw new SmartFlowProcessException("No process step found");
            }

            processStep = GetActiveProcessStep(userId);

            return processStep;
        }

        public ProcessResult FinalizeActiveProcessStep(ProcessStep processStep)
        {
            if (processStep == null)
            {
                return new ProcessResult
                {
                    Status = ProcessResultStatus.Failed,
                    Message = "No process step to finalize"
                };
            }

            _processRepository.SetToHistory(processStep.Entity.Id).Wait();
            var LastProcessStepHistoryItem = _processRepository.GetLastProcessStepHistoryItem(processStep.Entity.Id).Result;
            _entityCreateHistory.Create(LastProcessStepHistoryItem).Wait();
            var result = _processRepository.RemoveActiveProcessStep(processStep.Entity).Result;

            return new ProcessResult
            {
                Status = result ? ProcessResultStatus.Completed : ProcessResultStatus.Failed
            };
        }
    }
}
