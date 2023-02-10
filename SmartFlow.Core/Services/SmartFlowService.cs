using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using System;
using System.Threading.Tasks;
using SmartFlow.Core.Operators;
using System.Collections.Generic;

namespace SmartFlow.Core.Services
{
    public class SmartFlowService
    {
        private readonly ISmartFlowOperator _smartFlowOperator;
        private readonly ISmartFlowRepository _processRepository;
        private readonly TransitionService _transitionService;
        private readonly StateService _stateService;

        public SmartFlowService(ISmartFlowOperator smartFlowOperator
            , ISmartFlowRepository processRepository
            , TransitionService transitionService
            , StateService stateService
            )
        {
            _smartFlowOperator = smartFlowOperator;
            _processRepository = processRepository;
            _transitionService = transitionService;
            _stateService = stateService;
        }

        public async Task<Guid> Modify<T>(T smartFlow) where T : ISmartFlow, new()
        {
            await _smartFlowOperator.RegisterFlow(smartFlow);

            var processId = await _processRepository.Create<T>(smartFlow);
            if (processId == default)
            {
                throw new SmartFlowPersistencyException();
            }

            smartFlow.Transitions?.ForEach(transition =>
            {
                transition.ProcessId = processId;
                transition.From.Id = _stateService.Modify(transition.From).GetAwaiter().GetResult();
                transition.To.Id = _stateService.Modify(transition.To).GetAwaiter().GetResult();
                transition.Id = _transitionService.Modify(transition).GetAwaiter().GetResult();
            });

            return processId;
        }

        public async Task<List<ISmartFlow>> Get<T>(Guid processId = default, string key = default) where T : ISmartFlow
        {
            return await _processRepository.Get<T>(processId, key);
        }
    }
}
