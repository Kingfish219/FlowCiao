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
        private readonly IActionRepository _actionRepository;
        private readonly ActivityService _activityService;

        public TransitionService(ITransitionRepository transitionRepository
                        , IActionRepository actionRepository
                        , ActivityService activityService
            )
        {
            _transitionRepository = transitionRepository;
            _actionRepository = actionRepository;
            _activityService = activityService;
        }

        public async Task<Guid> Modify(Transition transition)
        {
            transition.Id = await _transitionRepository.Modify(transition);
            if (transition.Id == default)
            {
                throw new FlowCiaoPersistencyException();
            }

            transition.Activities?.ForEach(activity =>
            {
                var result = _activityService.Modify(activity).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("State Activity");
                }

                _transitionRepository.AssociateActivities(transition, activity).GetAwaiter().GetResult();
            });

            transition.Actions?.ForEach(action =>
            {
                var result = _actionRepository.Modify(action).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("State Activity");
                }

                _transitionRepository.AssociateActions(transition, action).GetAwaiter().GetResult();
            });

            return transition.Id;
        }
    }
}
