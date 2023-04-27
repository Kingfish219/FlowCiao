using System;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.SqlServer.Repositories;

namespace SmartFlow.Services
{
    public class StateService
    {
        private readonly StateRepository _stateRepository;
        private readonly ActivityService _activityService;

        public StateService(StateRepository stateRepository
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
