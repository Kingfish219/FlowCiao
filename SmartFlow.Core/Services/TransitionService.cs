using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Repositories;
using System.Threading.Tasks;
using System;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Services
{
    public class TransitionService
    {
        private readonly TransitionRepository _transitionRepository;
        private readonly ActionRepository _actionRepository;
        private readonly ActivityService _activityService;

        public TransitionService(TransitionRepository transitionRepository
                        , ActionRepository actionRepository
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
                throw new SmartFlowPersistencyException();
            }

            transition.Activities?.ForEach(activity =>
            {
                var result = _activityService.Modify(activity).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("State Activity");
                }

                _transitionRepository.AssociateActivities(transition, activity).GetAwaiter().GetResult();
            });

            transition.Actions?.ForEach(action =>
            {
                var result = _actionRepository.Modify(action).GetAwaiter().GetResult();
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("State Activity");
                }

                _transitionRepository.AssociateActions(transition, action).GetAwaiter().GetResult();
            });

            return transition.Id;
        }
    }
}
