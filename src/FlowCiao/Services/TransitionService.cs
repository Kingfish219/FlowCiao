using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Utils;

namespace FlowCiao.Services
{
    public class TransitionService
    {
        private readonly ITransitionRepository _transitionRepository;
        private readonly TriggerService _triggerService;
        private readonly ActivityService _activityService;

        public TransitionService(ITransitionRepository transitionRepository
                        , TriggerService triggerService
                        , ActivityService activityService
            )
        {
            _transitionRepository = transitionRepository;
            _triggerService = triggerService;
            _activityService = activityService;
        }

        public async Task<FuncResult<Guid>> Modify(Transition transition)
        {
            if (!transition.Activities.IsNullOrEmpty())
            {
                foreach (var activity in transition.Activities)
                {
                    var activityResult = await _activityService.Modify(activity);
                    if (activityResult == default)
                    {
                        return new FuncResult<Guid>(false, "Modifying Activity failed");
                    }
                }
            }
            
            var result = await _transitionRepository.Modify(transition);
            if (result == default)
            {
                return new FuncResult<Guid>(false, "Modifying Transition failed");
            }

            if (!transition.Triggers.IsNullOrEmpty())
            {
                foreach (var trigger in transition.Triggers)
                {
                    trigger.TransitionId = transition.Id;
                    var triggerResult = await _triggerService.Modify(trigger);
                    if (triggerResult == default)
                    {
                        return new FuncResult<Guid>(false, "Modifying Trigger failed");
                    }
                }
            }

            return new FuncResult<Guid>(true, data: result);
        }
    }
}
