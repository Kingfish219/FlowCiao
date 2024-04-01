using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Builder;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;
using FlowCiao.Services;
using FlowCiao.Utils;

namespace FlowCiao.Builders
{
    internal class FlowStepBuilder : IFlowStepBuilder
    {
        private FlowStep FlowStep { get; }
        public State InitialState { get; set; }
        public List<Action<Transition>> AllowedBuilders { get; set; }
        public List<Transition> Allowed { get; set; }
        public IFlowActivity OnEntryActivity { get; set; }
        public IFlowActivity OnExitActivity { get; set; }

        private readonly ActivityService _activityService;
        private readonly StateService _stateService;
        private readonly TransitionService _transitionService;

        public FlowStepBuilder(ActivityService activityService, StateService stateService,
            TransitionService transitionService)
        {
            _activityService = activityService;
            _stateService = stateService;
            _transitionService = transitionService;
            AllowedBuilders = new List<Action<Transition>>();
            FlowStep = new FlowStep();
        }

        public IFlowStepBuilder For(State state)
        {
            InitialState = state;

            return this;
        }

        public IFlowStepBuilder Allow(State state, int trigger, Func<bool> condition = null)
        {
            AllowedBuilders.Add((transition) =>
            {
                transition.From = InitialState;
                transition.To = state;
                transition.Activities = OnExitActivity != null
                    ? new List<Activity>
                    {
                        new(OnExitActivity)
                    }
                    : new List<Activity>();
                transition.Triggers = new List<Trigger>
                {
                    new(trigger)
                };
                transition.Condition = condition;
            });

            return this;
        }

        public IFlowStepBuilder OnEntry<TA>() where TA : IFlowActivity, new()
        {
            OnEntryActivity = (TA)Activator.CreateInstance(typeof(TA));
            var activity = new Activity(OnEntryActivity);
            InitialState.Activities ??= new List<Activity>();
            InitialState.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnEntry(string activityName)
        {
            var foundActivity = TryGetActivity(activityName);
            OnEntryActivity = foundActivity ??
                              throw new FlowCiaoException(
                                  $"Error in finding OnEntry activity. No type matches activity name {activityName} or the type is not derived from {nameof(IFlowActivity)}");
            var activity = new Activity(OnEntryActivity);
            InitialState.Activities ??= new List<Activity>();
            InitialState.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnExit<TA>() where TA : IFlowActivity, new()
        {
            OnExitActivity = (TA)Activator.CreateInstance(typeof(TA));

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

            OnExitActivity = (IFlowActivity)Activator.CreateInstance(activityType);

            return this;
        }

        public IFlowStepBuilder Build(List<State> states, JsonStep jsonStep)
        {
            var fromState = states.Single(state => state.Code == jsonStep.FromStateCode);
            var allowedList = states
                .Where(state => jsonStep.Allows.Exists(allowed => allowed.AllowedStateCode == state.Code))
                .Select(state => new
                {
                    AllowedState = state,
                    AllowedTrigger = jsonStep.Allows.Single(x => x.AllowedStateCode == state.Code).TriggerCode
                })
                .ToList();

            For(fromState);

            allowedList.ForEach(allowed => { Allow(allowed.AllowedState, allowed.AllowedTrigger); });

            if (jsonStep.OnEntry != null)
            {
                OnEntry(jsonStep.OnEntry.Name);
            }

            if (jsonStep.OnExit != null)
            {
                OnExit(jsonStep.OnExit.Name);
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
                    if (activityResult != default)
                    {
                        return new FuncResult(false, "Modifying Activity failed");
                    }
                }
            }

            var result = await _stateService.Modify(FlowStep.For);
            if (result != default)
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
                        if (activityResult != default)
                        {
                            return new FuncResult(false, "Modifying Activity failed");
                        }
                    }
                }
                
                var transitionResult = await _transitionService.Modify(transition);
                if (transitionResult != default)
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