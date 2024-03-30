using System;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Services
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
                throw new FlowCiaoPersistencyException("State");
            }

            state.Activities?.ForEach(activity =>
            {
                var result = _activityService.Modify(activity).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("State Activity");
                }

                _stateRepository.AssociateActivities(state, activity).GetAwaiter().GetResult();
            });

            return stateId;
        }
    }
}
