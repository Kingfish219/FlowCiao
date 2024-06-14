using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services.Interfaces;
using FlowCiao.Utils;

namespace FlowCiao.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly ActivityService _activityService;

        public StateService(IStateRepository stateRepository, ActivityService activityService)
        {
            _stateRepository = stateRepository;
            _activityService = activityService;
        }

        public async Task<FuncResult<Guid>> Modify(State state)
        {
            if (!state.Activities.IsNullOrEmpty())
            {
                foreach (var activity in state.Activities)
                {
                    var activityResult = await _activityService.Modify(activity);
                    if (activityResult == default)
                    {
                        return new FuncResult<Guid>(false, "Modifying Activity failed");
                    }
                }
            }
            
            var result = await _stateRepository.Modify(state);
            
            return new FuncResult<Guid>(true, data: result);
        }
    }
}
