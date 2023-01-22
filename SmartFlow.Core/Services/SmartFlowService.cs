using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SmartFlow.Core.Services
{
    public class SmartFlowService
    {
        private readonly ISmartFlowRepository _processRepository;
        private readonly TransitionRepository _transitionRepository;
        private readonly StateRepository _stateRepository;
        private readonly ActionRepository _actionRepository;

        public SmartFlowService(ISmartFlowRepository processRepository
            , TransitionRepository transitionRepository
            , StateRepository stateRepository
            , ActionRepository actionRepository
            )
        {
            _processRepository = processRepository;
            _transitionRepository = transitionRepository;
            _stateRepository = stateRepository;
            _actionRepository = actionRepository;
        }

        public async Task<Guid> Create<T>(ISmartFlow smartFlow) where T : ISmartFlow
        {
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
