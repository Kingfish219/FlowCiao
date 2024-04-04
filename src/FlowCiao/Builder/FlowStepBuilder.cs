using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Builder;
using FlowCiao.Models.Builder.Serialized;
using FlowCiao.Models.Core;
using FlowCiao.Services;
using FlowCiao.Utils;

namespace FlowCiao.Builder
{
    internal class FlowStepBuilder : IFlowStepBuilder
    {
        private FlowStep FlowStep { get; }

        private readonly ActivityService _activityService;
        private readonly StateService _stateService;
        private readonly TransitionService _transitionService;

        public FlowStepBuilder(ActivityService activityService, StateService stateService,
            TransitionService transitionService)
        {
            _activityService = activityService;
            _stateService = stateService;
            _transitionService = transitionService;
            FlowStep = new FlowStep();
        }

        public void AsInitialStep()
        {
            FlowStep.For.IsInitial = true;
        }

        public IFlowStepBuilder For(State state)
        {
            FlowStep.For = state;

            return this;
        }

        public IFlowStepBuilder Allow(State state, int trigger, Func<bool> condition = null)
        {
            FlowStep.Allowed.Add(new Transition
            {
                From = FlowStep.For,
                To = state,
                Activities = FlowStep.OnExit != null
                    ? new List<Activity>
                    {
                        new(FlowStep.OnExit)
                    }
                    : new List<Activity>(),
                Triggers = new List<Trigger>
                {
                    new(trigger)
                },
                Condition = condition
            });
            
            return this;
        }

        public IFlowStepBuilder OnEntry<TA>() where TA : IFlowActivity, new()
        {
            FlowStep.OnEntry = (TA)Activator.CreateInstance(typeof(TA));
            var activity = new Activity(FlowStep.OnEntry);
            FlowStep.For.Activities ??= new List<Activity>();
            FlowStep.For.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnEntry(string activityName)
        {
            var foundActivity = TryGetActivity(activityName);
            FlowStep.OnEntry = foundActivity ??
                               throw new FlowCiaoException(
                                   $"Error in finding OnEntry activity. No type matches activity name {activityName} or the type is not derived from {nameof(IFlowActivity)}");
            var activity = new Activity(FlowStep.OnEntry);
            FlowStep.For.Activities ??= new List<Activity>();
            FlowStep.For.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnExit<TA>() where TA : IFlowActivity, new()
        {
            FlowStep.OnExit = (TA)Activator.CreateInstance(typeof(TA));
            if (FlowStep.Allowed.IsNullOrEmpty())
            {
                throw new FlowCiaoException("No allowed transitions found in order to apply OnExit");
            }
            
            FlowStep.Allowed.ForEach(t =>
            {
                t.Activities ??= new List<Activity>();
                t.Activities.Add(new Activity(FlowStep.OnExit));
            });

            return this;
        }

        public IFlowStepBuilder OnExit(string activityName)
        {
            var activityType = GeneralUtils.FindType(activityName, typeof(IFlowActivity));
            if (activityType is null)
            {
                throw new FlowCiaoException(
                    $"Error in finding OnExit activity. No type matches activity name {activityName} or the type is not derived from {nameof(IFlowActivity)}");
            }

            FlowStep.OnExit = (IFlowActivity)Activator.CreateInstance(activityType);

            return this;
        }

        public IFlowStepBuilder Build(List<State> states, SerializedStep serializedStep)
        {
            var fromState = states.Single(state => state.Code == serializedStep.FromStateCode);
            var allowedList = states
                .Where(state => serializedStep.Allows.Exists(allowed => allowed.AllowedStateCode == state.Code))
                .Select(state => new
                {
                    AllowedState = state,
                    AllowedTrigger = serializedStep.Allows.Single(x => x.AllowedStateCode == state.Code).TriggerCode
                })
                .ToList();

            For(fromState);

            allowedList.ForEach(allowed => { Allow(allowed.AllowedState, allowed.AllowedTrigger); });

            if (serializedStep.OnEntry != null)
            {
                OnEntry(serializedStep.OnEntry.Name);
            }

            if (serializedStep.OnExit != null)
            {
                OnExit(serializedStep.OnExit.Name);
            }

            return this;
        }

        public FlowStep Build()
        {
            var result = Persist().GetAwaiter().GetResult();
            if (!result.Success)
            {
                throw new FlowCiaoPersistencyException("Saving some data failed");
            }

            return FlowStep;
        }

        public void Rollback()
        {
            // ignored
        }

        private async Task<FuncResult> Persist()
        {
            if (!FlowStep.For.Activities.IsNullOrEmpty())
            {
                foreach (var activity in FlowStep.For.Activities)
                {
                    var activityResult = await _activityService.Modify(activity);
                    if (activityResult == default)
                    {
                        return new FuncResult(false, "Modifying Activity failed");
                    }
                }
            }

            var result = await _stateService.Modify(FlowStep.For);
            if (result == default)
            {
                return new FuncResult(false, "Modifying State failed");
            }

            if (FlowStep.Allowed.IsNullOrEmpty())
            {
                return new FuncResult(true);
            }

            foreach (var transition in FlowStep.Allowed)
            {
                if (!transition.Activities.IsNullOrEmpty())
                {
                    foreach (var activity in transition.Activities)
                    {
                        var activityResult = await _activityService.Modify(activity);
                        if (activityResult == default)
                        {
                            return new FuncResult(false, "Modifying Activity failed");
                        }
                    }
                }
                
                var transitionResult = await _transitionService.Modify(transition);
                if (transitionResult == default)
                {
                    return new FuncResult(false, "Modifying Transition failed");
                }
            }

            return new FuncResult(true);
        }

        private IFlowActivity TryGetActivity(string activityName)
        {
            try
            {
                IFlowActivity flowActivity;

                var activityType = GeneralUtils.FindType(activityName, typeof(IFlowActivity));
                if (activityType != null)
                {
                    flowActivity = (IFlowActivity)Activator.CreateInstance(activityType);
                }
                else
                {
                    var storedActivity = _activityService.LoadActivity(activityName).GetAwaiter().GetResult();
                    flowActivity = storedActivity;
                }

                return flowActivity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}