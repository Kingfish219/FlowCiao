using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Operators;
using SmartFlow.Persistence.Interfaces;
using Process = SmartFlow.Models.Flow.Process;

namespace SmartFlow.Services
{
    public class ProcessExecutionService
    {
        private readonly IProcessExecutionRepository _processExecutionRepository;
        private readonly SmartFlowSettings _smartFlowSettings;
        private readonly SmartFlowHub _smartFlowHub;

        public ProcessExecutionService(IProcessExecutionRepository processExecutionRepository
            , SmartFlowSettings smartFlowSettings
            , SmartFlowHub smartFlowHub
            )
        {
            _smartFlowSettings = smartFlowSettings;
            _processExecutionRepository = processExecutionRepository;
            _smartFlowHub = smartFlowHub;
        }

        public async Task<List<ProcessExecution>> Get(Guid id = default, Guid processId = default)
        {
            return await _processExecutionRepository.Get(id, processId);
        }

        private ProcessExecutionStep GenerateProcessStep(Process process, State state)
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

        public async Task<ProcessExecution> InitializeProcessExecution(Process process)
        {
            var processExecution = new ProcessExecution
            {
                Id = Guid.NewGuid(),
                Process = process,
                CreatedOn = DateTime.Now,
                ExecutionState = ProcessExecution.ProcessExecutionState.Initial,
                ExecutionSteps = new List<ProcessExecutionStep>
                {
                    GenerateProcessStep(process,
                            process.Transitions.First(x => x.From.IsInitial).From)
                },
                State = process.InitialState
            };

            if (_smartFlowSettings.PersistFlow)
            {
                await Modify(processExecution);
            }

            return processExecution;
        }

        public async Task<ProcessExecution> FinalizeProcessExecutionStep(Process process)
        {
            var processExecution = new ProcessExecution
            {
                Id = Guid.NewGuid(),
                Process = process,
                CreatedOn = DateTime.Now,
                ExecutionState = ProcessExecution.ProcessExecutionState.Initial,
                ExecutionSteps = new List<ProcessExecutionStep>
                {
                    GenerateProcessStep(process,
                        process.Transitions.First(x => x.From.IsInitial).From)
                }
            };

            if (_smartFlowSettings.PersistFlow)
            {
                await Modify(processExecution);
            }

            return processExecution;
        }

        public async Task<ProcessExecution> Finalize(ProcessExecution processExecution)
        {
            processExecution.ExecutionSteps.Add(GenerateProcessStep(processExecution.Process,
                        processExecution.Process.Transitions.First(x => x.From.IsInitial).From));

            if (_smartFlowSettings.PersistFlow)
            {
                await Modify(processExecution);
            }

            return processExecution;
        }

        public async Task<Guid> Modify(ProcessExecution entity)
        {
            var processId = await _processExecutionRepository.Modify(entity);
            if (processId == default)
            {
                throw new SmartFlowPersistencyException();
            }

            return processId;
        }
    }
}
