﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Models;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;
using Process = FlowCiao.Models.Flow.Process;

namespace FlowCiao.Services
{
    public class ProcessExecutionService
    {
        private readonly IProcessExecutionRepository _processExecutionRepository;
        private readonly FlowSettings _flowSettings;

        public ProcessExecutionService(IProcessExecutionRepository processExecutionRepository
            , FlowSettings flowSettings
            )
        {
            _flowSettings = flowSettings;
            _processExecutionRepository = processExecutionRepository;
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

            if (_flowSettings.PersistFlow)
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

            if (_flowSettings.PersistFlow)
            {
                await Modify(processExecution);
            }

            return processExecution;
        }

        public async Task<ProcessExecution> Finalize(ProcessExecution processExecution)
        {
            processExecution.ExecutionSteps.Add(GenerateProcessStep(processExecution.Process,
                        processExecution.Process.Transitions.First(x => x.From.IsInitial).From));

            if (_flowSettings.PersistFlow)
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
                throw new FlowCiaoPersistencyException();
            }

            return processId;
        }
    }
}
