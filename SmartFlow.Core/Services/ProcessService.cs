using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using System;
using System.Threading.Tasks;
using SmartFlow.Core.Operators;
using System.Collections.Generic;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;

namespace SmartFlow.Core.Services
{
    public class ProcessService
    {
        private readonly ISmartFlowOperator _smartFlowOperator;
        private readonly IProcessRepository _processRepository;
        private readonly TransitionService _transitionService;
        private readonly StateService _stateService;

        public ProcessService(ISmartFlowOperator smartFlowOperator
            , IProcessRepository processRepository
            , TransitionService transitionService
            , StateService stateService
            )
        {
            _smartFlowOperator = smartFlowOperator;
            _processRepository = processRepository;
            _transitionService = transitionService;
            _stateService = stateService;
        }

        public async Task<Guid> Modify(Process process)
        {
            await _smartFlowOperator.RegisterFlow(process);

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
            return await _processRepository.Get(processId, key);
        }
    }
}
