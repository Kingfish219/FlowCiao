using System;
using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Services
{
    public class ProcessStepService : IProcessStepService
    {
        private readonly IStateMachineRepository _processRepository;

        public ProcessStepService(IStateMachineRepository processRepository)
        {
            _processRepository = processRepository;
        }

        public ProcessStep GetActiveProcessStep(Guid userId, ProcessEntity entity)
        {
            var processStep = _processRepository.GetActiveProcessStep(entity).Result;
            if (processStep != null)
            {
                var process = _processRepository.GetProcess(processStep.Process.Id).Result;
                var transitionActions = _processRepository.GetActiveTransitions(entity, process.Id).Result;
                processStep.Entity = entity;
                processStep.TransitionActions = transitionActions;
                processStep.Process = process;
            }

            return processStep;
        }

        //public ProcessStep InitializeActiveProcessStep(Guid userID)
        public ProcessStep InitializeActiveProcessStep(Guid userId, ProcessEntity entity, bool initializeFromFirstState = false)
        {
            var process = _processRepository.GetProcess(userId).Result;
            State state;
            if (initializeFromFirstState)
            {
                state = _processRepository.GetStartState();
            }
            else
            {
                state = new State { Id = entity.LastStatus };
            }

            var transitionActions = _processRepository.GetStateTransitions(process, state).Result;
            var processStep = new ProcessStep
            {
                Process = process,
                TransitionActions = transitionActions,
                Entity = entity
            };

            var result = _processRepository.CreateProcessStep(processStep).Result;
            if (!result)
            {
                throw new SmartFlowProcessExecutionException("No process step found");
            }

            processStep = GetActiveProcessStep(userId, entity);

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
            var result = _processRepository.RemoveActiveProcessStep(processStep.Entity).Result;

            return new ProcessResult
            {
                Status = result ? ProcessResultStatus.Completed : ProcessResultStatus.Failed
            };
        }
    }
}
