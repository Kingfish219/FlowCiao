﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly TransitionService _transitionService;
        private readonly StateService _stateService;

        public ProcessService(IProcessRepository processRepository
            , TransitionService transitionService
            , StateService stateService
            )
        {
            _processRepository = processRepository;
            _transitionService = transitionService;
            _stateService = stateService;
        }

        public async Task<Guid> Modify(Process process)
        {
            var processId = await _processRepository.Modfiy(process);
            if (processId == default)
            {
                throw new SmartFlowPersistencyException();
            }

            process.Transitions?.ForEach(async transition =>
            {
                transition.ProcessId = processId;
                transition.From.Id = await _stateService.Modify(transition.From);
                transition.To.Id = await _stateService.Modify(transition.To);
                transition.Id = await _transitionService.Modify(transition);
            });

            return processId;
        }

        public async Task<List<Process>> Get(Guid processId = default, string key = default)
        {
            return await _processRepository.Get(processId, key);
        }

        public ProcessExecutionStep GenerateProcessStep(Process process, State state)
        {
            var processStep = new ProcessExecutionStep
            {
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

        public async Task<ProcessExecution> Finalize(ProcessExecution processExecution)
        {
            await Task.CompletedTask;

            processExecution.ExecutionSteps.Add(GenerateProcessStep(processExecution.Process,
                        processExecution.Process.Transitions.First(x => x.From.IsInitial).From));

            return processExecution;
        }
    }
}
