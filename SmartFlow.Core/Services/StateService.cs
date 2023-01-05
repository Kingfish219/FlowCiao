using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System.Threading.Tasks;
using System;

namespace SmartFlow.Core.Services
{
    public class StateService
    {
        private readonly StateRepository _stateRepository;
        private readonly ActivityService _activityService;

        public StateService(StateRepository stateRepository
                        , ActionRepository actionRepository
                        , ActivityService activityService
            )
        {
            _stateRepository = stateRepository;
            _activityService = activityService;
        }

        public async Task<Guid> Create(State state)
        {
            var stateId = await _stateRepository.Create(state).ConfigureAwait(false);
            if (stateId == default)
            {
                throw new SmartFlowPersistencyException("State");
            }

            state.Activities.ForEach(async activity =>
            {
                var result = await _activityService.Create(activity).ConfigureAwait(false);
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("State Activity");
                }
            });

            return stateId;
        }
    }
}
