using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Repositories;
using System;
using System.Threading.Tasks;
using SmartFlow.Core.Operators;

namespace SmartFlow.Core.Services
{
    public class SmartFlowService
    {
        private readonly ISmartFlowOperator _smartFlowOperator;
        private readonly ISmartFlowRepository _processRepository;
        private readonly TransitionRepository _transitionRepository;
        private readonly StateRepository _stateRepository;
        private readonly ActionRepository _actionRepository;

        public SmartFlowService(ISmartFlowOperator smartFlowOperator
            , ISmartFlowRepository processRepository
            , TransitionRepository transitionRepository
            , StateRepository stateRepository
            , ActionRepository actionRepository
            )
        {
            _processRepository = processRepository;
            _transitionRepository = transitionRepository;
            _stateRepository = stateRepository;
            _actionRepository = actionRepository;
            _smartFlowOperator = smartFlowOperator;
        }

        public async Task<Guid> Create<T>(T smartFlow) where T : ISmartFlow, new()
        {
            await _smartFlowOperator.RegisterFlow(smartFlow);

            smartFlow.Transitions.ForEach(transition =>
            {
                transition.From.Id = _stateRepository.Create(transition.From).GetAwaiter().GetResult();
                transition.To.Id = _stateRepository.Create(transition.To).GetAwaiter().GetResult();
                transition.Id = _transitionRepository.Create(transition).GetAwaiter().GetResult();
            });

            var processId = await _processRepository.Create<T>(smartFlow);
            if (processId == default)
            {
                throw new SmartFlowPersistencyException();
            }

            return processId;
        }

        public async Task<ISmartFlow> GetProcess<T>(Guid processId = default, string key = default) where T : ISmartFlow
        {
            return await _processRepository.GetProcess<T>(processId, key);
        }
    }
}
