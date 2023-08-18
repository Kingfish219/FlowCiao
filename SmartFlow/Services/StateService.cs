using System;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Services
{
    public class StateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly ActivityService _activityService;

        public StateService(IStateRepository stateRepository
                        , ActivityService activityService
            )
        {
            _stateRepository = stateRepository;
            _activityService = activityService;
        }

        public async Task<Guid> Modify(State state)
        {
            var stateId = await _stateRepository.Modify(state);
            if (stateId == default)
            {
                throw new SmartFlowPersistencyException("State");
            }

            state.Activities?.ForEach(activity =>
            {
                var result = _activityService.Modify(activity).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("State Activity");
                }

                _stateRepository.AssociateActivities(state, activity).GetAwaiter().GetResult();
            });

            return stateId;
        }
    }
}
