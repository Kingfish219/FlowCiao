using System;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Utils;

namespace FlowCiao.Services
{
    public class StateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly ActivityService _activityService;

        public StateService(IStateRepository stateRepository, ActivityService activityService)
        {
            _stateRepository = stateRepository;
            _activityService = activityService;
        }

        public async Task<Guid> Modify(State state)
        {
            if (!state.Activities.IsNullOrEmpty())
            {
                foreach (var activity in state.Activities)
                {
                    var activityResult = await _activityService.Modify(activity);
                    if (activityResult == default)
                    {
                        throw new FlowCiaoPersistencyException("Modifying Activity failed");
                    }
                }
            }
            
            return await _stateRepository.Modify(state);
        }
    }
}
