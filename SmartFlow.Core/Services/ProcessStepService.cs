using System;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;

namespace SmartFlow.Core.Services
{
    public class ProcessStepService : IProcessStepService
    {
        private readonly IProcessRepository _smartFlowRepository;

        public ProcessStepService(IProcessRepository smartFlowRepository)
        {
            _smartFlowRepository = smartFlowRepository;
        }

        public ProcessStep GetActiveProcessStep(Guid userId, ProcessEntity entity)
        {
            var processStep = _smartFlowRepository.GetActiveProcessStep(entity).Result;
            if (processStep != null)
            {
                var process = _smartFlowRepository.GetProcess(processStep.Process.Id).Result;
                var transitionActions = _smartFlowRepository.GetActiveTransitions(entity, process.Id).Result;
                processStep.Entity = entity;
                processStep.TransitionActions = transitionActions;
                processStep.Process = process;
            }

            return processStep;
        }

        //public ProcessStep InitializeActiveProcessStep(Guid userID)
        public ProcessStep InitializeActiveProcessStep(Guid userId, ProcessEntity entity, bool initializeFromFirstState = false)
        {
            var process = _smartFlowRepository.GetProcess(userId).Result;
            State state;
            if (initializeFromFirstState)
            {
                state = _smartFlowRepository.GetStartState();
            }
            else
            {
                state = new State { Id = entity.LastStatus };
            }

            var transitionActions = _smartFlowRepository.GetStateTransitions(process, state).Result;
            var processStep = new ProcessStep
            {
                Process = process,
                TransitionActions = transitionActions,
                Entity = entity
            };

            var result = _smartFlowRepository.CreateProcessStep(processStep).Result;
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

            _smartFlowRepository.SetToHistory(processStep.Entity.Id).Wait();
            var result = _smartFlowRepository.RemoveActiveProcessStep(processStep.Entity).Result;

            return new ProcessResult
            {
                Status = result ? ProcessResultStatus.Completed : ProcessResultStatus.Failed
            };
        }
    }
}
