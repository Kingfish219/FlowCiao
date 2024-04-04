using System;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Services
{
    public class TransitionService
    {
        private readonly ITransitionRepository _transitionRepository;
        private readonly ITriggerRepository _triggerRepository;
        private readonly ActivityService _activityService;

        public TransitionService(ITransitionRepository transitionRepository
                        , ITriggerRepository triggerRepository
                        , ActivityService activityService
            )
        {
            _transitionRepository = transitionRepository;
            _triggerRepository = triggerRepository;
            _activityService = activityService;
        }

        public async Task<Guid> Modify(Transition transition)
        {
            return await _transitionRepository.Modify(transition);

            // transition.Id = await _transitionRepository.Modify(transition);
            // if (transition.Id == default)
            // {
            //     throw new FlowCiaoPersistencyException();
            // }
            //
            // transition.Activities?.ForEach(activity =>
            // {
            //     var result = _activityService.Modify(activity).GetAwaiter().GetResult();
            //     if (result == default)
            //     {
            //         throw new FlowCiaoPersistencyException("State Activity");
            //     }
            //
            //     _transitionRepository.AssociateActivities(transition, activity).GetAwaiter().GetResult();
            // });
            //
            // transition.Triggers?.ForEach(trigger =>
            // {
            //     var result = _triggerRepository.Modify(trigger).GetAwaiter().GetResult();
            //     if (result == default)
            //     {
            //         throw new FlowCiaoPersistencyException("State Activity");
            //     }
            //
            //     _transitionRepository.AssociateTriggers(transition, trigger).GetAwaiter().GetResult();
            // });
            //
            // return transition.Id;
        }
    }
}
