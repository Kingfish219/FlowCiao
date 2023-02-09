using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Repositories;
using System.Threading.Tasks;
using System;
using SmartFlow.Core.Models;
using System.Diagnostics;

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

        public async Task<Guid> Modify(Transition transition)
        {
            transition.From.Id = await _stateService.Modify(transition.From);
            if (transition.From.Id == default)
            {
                throw new SmartFlowPersistencyException("Transition From");
            }

            transition.To.Id = await _stateService.Modify(transition.To);
            if (transition.To.Id == default)
            {
                throw new SmartFlowPersistencyException("Transition To");
            }

            transition.Activities.ForEach(async activity =>
            {
                var result = await _activityService.Modify(activity).ConfigureAwait(false);
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("State Activity");
                }
            });

            transition.Actions.ForEach(async action =>
            {
                var result = await _actionRepository.Modify(activity).ConfigureAwait(false);
                if (result == default)
                {
                    throw new SmartFlowPersistencyException("State Activity");
                }

                await _transitionRepository.CreateActions(transition, action);
            });

            transition.Id = _transitionRepository.Modify(transition).GetAwaiter().GetResult();

            return transition.Id;
        }
    }
}
