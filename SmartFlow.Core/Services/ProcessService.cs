using SmartFlow.Core.Db;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace SmartFlow.Core.Services
{
    public class ProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly TransitionRepository _transitionRepository;
        private readonly StateRepository _stateRepository;
        private readonly ActionRepository _actionRepository;

        public ProcessService(IProcessRepository processRepository
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

        public async Task<Guid> Create(Process process)
        {
            var processId = await _processRepository.Create(process);
            if (processId == default)
            {
                throw new SmartFlowPersistencyException();
            }

            return processId;
        }

        public async Task<Process> GetProcess(Guid processId = default, string key = default)
        {
            return await _processRepository.GetProcess(processId, key);
        }
    }
}
