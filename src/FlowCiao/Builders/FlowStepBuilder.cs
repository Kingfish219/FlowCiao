using System;
using System.Collections.Generic;
using System.Linq;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Utils;

namespace FlowCiao.Builders
{
    internal class FlowStepBuilder : IFlowStepBuilder
    {
        private readonly IActivityRepository _activityRepository;
        public IFlowBuilder FlowBuilder { get; set; }

        public FlowStepBuilder(IFlowBuilder flowBuilder, IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
            FlowBuilder = flowBuilder;
            AllowedTransitionsBuilders = new List<Action<Transition>>();
        }

        public State InitialState { get; set; }
        public List<Action<Transition>> AllowedTransitionsBuilders { get; set; }
        public IFlowActivity OnEntryActivity { get; set; }
        public IFlowActivity OnExitActivity { get; set; }

        public IFlowStepBuilder For(State state)
        {
            InitialState = state;

            return this;
        }

        public IFlowStepBuilder Allow(State state, List<int> triggers)
        {
            AllowedTransitionsBuilders.Add((transition) =>
            {
                transition.From = InitialState;
                transition.To = state;
                transition.Activities = OnExitActivity != null
                    ? new List<Activity>
                    {
                        new(OnExitActivity)
                    }
                    : new List<Activity>();
                transition.Triggers = triggers.Select(trigger => new Trigger(trigger)).ToList();
            });

            return this;
        }

        public IFlowStepBuilder Allow(State state, int trigger, Func<bool> condition = null)
        {
            AllowedTransitionsBuilders.Add((transition) =>
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

        public IFlowStepBuilder AllowSelf(List<int> triggers)
        {
            throw new NotImplementedException();
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

        public IFlowStepBuilder AssignToUser(Func<string> userId)
        {
            InitialState.OwnerId = userId();

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

        public void Rollback()
        {
            // ignored
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
                    var storedActivity = _activityRepository.LoadActivity(activityName).GetAwaiter().GetResult();
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