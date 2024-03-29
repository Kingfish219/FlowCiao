﻿using System;
using System.Collections.Generic;
using System.Linq;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Flow;
using FlowCiao.Utils;

namespace FlowCiao.Builders
{
    internal class FlowStepBuilder : IFlowStepBuilder
    {
        public IFlowBuilder FlowBuilder { get; set; }

        public FlowStepBuilder(IFlowBuilder flowBuilder)
        {
            FlowBuilder = flowBuilder;
            AllowedTransitionsBuilders = new List<Action<Transition>>();
        }

        public State InitialState { get; set; }
        public List<Action<Transition>> AllowedTransitionsBuilders { get; set; }
        public IProcessActivity OnEntryActivity { get; set; }
        public IProcessActivity OnExitActivity { get; set; }

        public IFlowStepBuilder From(State state)
        {
            InitialState = state;

            return this;
        }

        public IFlowStepBuilder Allow(State state, List<int> actions)
        {
            AllowedTransitionsBuilders.Add((transition) =>
            {
                transition.From = InitialState;
                transition.To = state;
                transition.Activities = OnExitActivity != null
                    ? new List<Activity>
                    {
                        new()
                        {
                            ProcessActivityExecutor = OnExitActivity
                        }
                    }
                    : new List<Activity>();
                transition.Actions = actions.Select(action => new ProcessAction(action)).ToList();
            });

            return this;
        }

        public IFlowStepBuilder Allow(State state, int action, Func<bool> condition = null)
        {
            AllowedTransitionsBuilders.Add((transition) =>
            {
                transition.From = InitialState;
                transition.To = state;
                transition.Activities = OnExitActivity != null
                    ? new List<Activity>
                    {
                        new()
                        {
                            ProcessActivityExecutor = OnExitActivity
                        }
                    }
                    : new List<Activity>();
                transition.Actions = new List<ProcessAction>
                {
                    new ProcessAction(action)
                };
                transition.Condition = condition;
            });

            return this;
        }

        public IFlowStepBuilder AllowSelf(List<int> actions)
        {
            throw new NotImplementedException();
        }

        public IFlowStepBuilder OnEntry<TA>() where TA : IProcessActivity, new()
        {
            OnEntryActivity = (TA)Activator.CreateInstance(typeof(TA));
            var activity = new Activity
            {
                ProcessActivityExecutor = OnEntryActivity
            };
            InitialState.Activities ??= new List<Activity>();
            InitialState.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnEntry(string activityName)
        {
            var activityType = GeneralUtils.FindType(activityName, typeof(IProcessActivity));
            if (activityType is null)
            {
                throw new FlowCiaoException(
                    $"Error in finding OnEntry activity. No type matches activity name {activityName} or the type is not derived from {nameof(IProcessActivity)}");
            }

            OnEntryActivity = (IProcessActivity)Activator.CreateInstance(activityType);
            var activity = new Activity
            {
                ProcessActivityExecutor = OnEntryActivity
            };
            InitialState.Activities ??= new List<Activity>();
            InitialState.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnExit<TA>() where TA : IProcessActivity, new()
        {
            OnExitActivity = (TA)Activator.CreateInstance(typeof(TA));

            return this;
        }

        public IFlowStepBuilder OnExit(string activityName)
        {
            var activityType = GeneralUtils.FindType(activityName, typeof(IProcessActivity));
            if (activityType is null)
            {
                throw new FlowCiaoException(
                    $"Error in finding OnExit activity. No type matches activity name {activityName} or the type is not derived from {nameof(IProcessActivity)}");
            }

            OnExitActivity = (IProcessActivity)Activator.CreateInstance(activityType);

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
                    AllowedAction = jsonStep.Allows.Single(x => x.AllowedStateCode == state.Code).ActionCode
                })
                .ToList();

            From(fromState);

            allowedList.ForEach(allowed =>
            {
                Allow(allowed.AllowedState, allowed.AllowedAction);
            });

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
    }
}