using System;
using System.Collections.Generic;
using System.Linq;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Flow;

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
        public IProcessActivity OnEntryActivty { get; set; }
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
            OnEntryActivty = (TA)Activator.CreateInstance(typeof(TA));
            var activity = new Activity
            {
                ProcessActivityExecutor = OnEntryActivty
            };
            InitialState.Activities ??= new List<Activity>();
            InitialState.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder OnExit<TA>() where TA : IProcessActivity, new()
        {
            OnExitActivity = (TA)Activator.CreateInstance(typeof(TA));
            //var activity = new Activity
            //{
            //    ProcessActivityExecutor = OnExitActivity
            //};
            //InitialState.Activities ??= new List<Activity>();
            //InitialState.Activities.Add(activity);

            return this;
        }

        public IFlowStepBuilder AssignToUser(Func<string> userId)
        {
            InitialState.OwnerId = userId();

            return this;
        }

        public IFlowStepBuilder Build(List<State> states, JsonStep jsonStep)
        {
            var fromState = states.Single(state => state.Code == jsonStep.From.Code);
            var allowedList = states
                .Where(state => jsonStep.Allows.Exists(allowed => allowed.Item1.Code == state.Code))
                .Select(state => new
                {
                    AllowedState = state,
                    AllowedAction = jsonStep.Allows.Single(x => x.Item1.Code == state.Code).Item2
                })
                .ToList();

            From(fromState);

            allowedList.ForEach(allowed =>
            {
                Allow(allowed.AllowedState, allowed.AllowedAction);
            });

            return this;
        }

        public void Rollback()
        {
            // ignored
        }
    }
}