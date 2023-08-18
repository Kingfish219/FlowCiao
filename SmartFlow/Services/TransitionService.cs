using System;
using System.Threading.Tasks;
using SmartFlow.Exceptions;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.SqlServer.Repositories;

namespace SmartFlow.Services
{
    public class TransitionService
    {
        private readonly TransitionRepository _transitionRepository;
        private readonly ActionCacheRepository _actionRepository;
        private readonly ActivityService _activityService;

        public TransitionService(TransitionRepository transitionRepository
                        , ActionCacheRepository actionRepository
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
