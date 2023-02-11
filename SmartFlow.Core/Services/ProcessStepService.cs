using System;
using System.Collections.Generic;
using System.Linq;
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

        public ProcessExecutionStep GenerateProcessStep(Process process, State state)
        {
            var processStep = new ProcessExecutionStep
            {
                Process = process,
                CreatedOn = DateTime.Now,
                Details = process.Transitions.Where(x => x.From.Id.Equals(state.Id))
                    .Select(transition =>
                    {
                        return new ProcessExecutionStepDetail
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            Transition = transition
                        };
                    }).ToList()
            };

            return processStep;
        }

        //public ProcessExecutionStep GetActiveProcessStep(Guid userId, ProcessEntity entity)
        //{
        //    var processStep = _smartFlowRepository.GetActiveProcessStep(entity).Result;
        //    if (processStep != null)
        //    {
        //        var process = _smartFlowRepository.GetProcess(processStep.Process.Id).Result;
        //        var transitionActions = _smartFlowRepository.GetActiveTransitions(entity, process.Id).Result;
        //        processStep.Entity = entity;
        //        processStep.TransitionActions = transitionActions;
        //        processStep.Process = process;
        //    }

        //    return processStep;
        //}

        //public ProcessExecutionStep InitializeActiveProcessStep(Process process)
        //{
        //    var processStep = new ProcessExecutionStep
        //    {
        //        Process = process
        //    };

        //    var result = _smartFlowRepository.CreateProcessStep(processStep).Result;
        //    if (!result)
        //    {
        //        throw new SmartFlowProcessExecutionException("No process step found");
        //    }

        //    return processStep;
        //}

        //public ProcessExecutionStep InitializeActiveProcessStep(Guid userId, ProcessEntity entity, bool initializeFromFirstState = false)
        //{
        //    var process = _smartFlowRepository.GetProcess(userId).Result;
        //    State state;
        //    if (initializeFromFirstState)
        //    {
        //        state = _smartFlowRepository.GetStartState();
        //    }
        //    else
        //    {
        //        state = new State { Id = entity.LastStatus };
        //    }

        //    var transitionActions = _smartFlowRepository.GetStateTransitions(process, state).Result;
        //    var processStep = new ProcessExecutionStep
        //    {
        //        Process = process,
        //        TransitionActions = transitionActions,
        //        Entity = entity
        //    };

        //    var result = _smartFlowRepository.CreateProcessStep(processStep).Result;
        //    if (!result)
        //    {
        //        throw new SmartFlowProcessExecutionException("No process step found");
        //    }

        //    processStep = GetActiveProcessStep(userId, entity);

        //    return processStep;
        //}

        public ProcessResult FinalizeActiveProcessStep(ProcessExecutionStep processStep)
        {
            //if (processStep == null)
            //{
            //    return new ProcessResult
            //    {
            //        Status = ProcessResultStatus.Failed,
            //        Message = "No process step to finalize"
            //    };
            //}

            //_smartFlowRepository.SetToHistory(processStep.Entity.Id).Wait();
            //var result = _smartFlowRepository.RemoveActiveProcessStep(processStep.Entity).Result;

            //return new ProcessResult
            //{
            //    Status = result ? ProcessResultStatus.Completed : ProcessResultStatus.Failed
            //};

            return new ProcessResult();
        }
    }
}
