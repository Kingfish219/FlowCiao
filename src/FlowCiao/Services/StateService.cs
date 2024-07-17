using System;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Utils;

namespace FlowCiao.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly IActivityService _activityService;

        public StateService(IStateRepository stateRepository, IActivityService activityService)
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
