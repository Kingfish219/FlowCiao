using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Operators;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly TransitionService _transitionService;
        private readonly StateService _stateService;
        private readonly SmartFlowSettings _smartFlowSettings;
        private readonly SmartFlowHub _smartFlowHub;

        public ProcessService(IProcessRepository processRepository
            , TransitionService transitionService
            , StateService stateService
            , SmartFlowSettings smartFlowSettings
            , SmartFlowHub smartFlowHub
            )
        {
            _processRepository = processRepository;
            _transitionService = transitionService;
            _stateService = stateService;
            _smartFlowSettings = smartFlowSettings;
            _smartFlowHub = smartFlowHub;
        }

        public async Task<Guid> Modify(Process process)
        {
            var processId = await _processRepository.Create(process);
            if (processId == default)
            {
                throw new SmartFlowPersistencyException();
            }

            process.Transitions?.ForEach(transition =>
            {
                transition.ProcessId = processId;
                transition.From.Id = _stateService.Modify(transition.From).GetAwaiter().GetResult();
                transition.To.Id = _stateService.Modify(transition.To).GetAwaiter().GetResult();
                transition.Id = _transitionService.Modify(transition).GetAwaiter().GetResult();
            });

            return processId;
        }

        public async Task<List<Process>> Get(Guid processId = default, string key = default)
        {
            return await _smartFlowHub.RetreiveFlow(key);
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
            processExecution.ExecutionSteps.Add(GenerateProcessStep(processExecution.Process,
                        processExecution.Process.Transitions.First(x => x.From.IsInitial).From));

            //if (_smartFlowSettings.PersistFlow)
            //{
            //    await Modify(processExecution);
            //}

            return processExecution;
        }
    }
}
