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
        private readonly StateService _stateService;
        private readonly ActionRepository _actionRepository;
        private readonly ActivityService _activityService;

        public TransitionService(TransitionRepository transitionRepository
                        , StateService stateService
                        , ActionRepository actionRepository
                        , ActivityService activityService
            )
        {
            _transitionRepository = transitionRepository;
            _stateService = stateService;
            _actionRepository = actionRepository;
            _activityService = activityService;
        }

        public async Task<Guid> Create(Transition transition)
        {
            var transitionId = await _transitionRepository.Create(transition);
            if (transitionId == default)
            {
                throw new SmartFlowPersistencyException("Transition");
            }

            transition.Id = transitionId;
            var fromId = await _stateService.Create(transition.From);
            if (fromId == default)
            {
                throw new SmartFlowPersistencyException("Transition From");
            }

            var toId = await _stateService.Create(transition.To);
            if (toId == default)
            {
                throw new SmartFlowPersistencyException("Transition To");
            }

            transition.Activities.ForEach(async activity =>
            {
                var result = await _activityService.Create(activity).ConfigureAwait(false);
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("State Activity");
                }
            });

            return transitionId;
        }
    }
}
