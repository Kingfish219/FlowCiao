using System;
using System.Collections.Generic;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Builders
{
    public class SmartFlowStepBuilder : ISmartFlowStepBuilder
    {
        public ISmartFlowBuilder SmartFlowBuilder { get; set; }

        public SmartFlowStepBuilder(ISmartFlowBuilder smartFlowBuilder)
        {
            SmartFlowBuilder = smartFlowBuilder;
            AllowedTransitions = new List<(State, List<ProcessAction>)>();
        }

        public State InitialState { get; set; }
        public List<(State, List<ProcessAction>)> AllowedTransitions { get; set; }
        public IProcessActivity OnEntryActivty { get; set; }
        public IProcessActivity OnExitActivity { get; set; }

        public ISmartFlowStepBuilder From(State state)
        {
            InitialState = state;

            return this;
        }

        public ISmartFlowStepBuilder Allow(State state, List<ProcessAction> actions)
        {
            AllowedTransitions.Add((state, actions));

            return this;
        }

        public ISmartFlowStepBuilder Allow(State state, ProcessAction action)
        {
            var actions = new List<ProcessAction>
            {
                action
            };

            AllowedTransitions.Add((state, actions));

            return this;
        }

        public ISmartFlowStepBuilder AllowSelf(List<ProcessAction> actions)
        {
            throw new NotImplementedException();
        }

        public ISmartFlowStepBuilder OnEntry<TA>() where TA : IProcessActivity, new()
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

        public ISmartFlowStepBuilder OnExit<TA>() where TA : IProcessActivity, new()
        {
            OnExitActivity = (TA)Activator.CreateInstance(typeof(TA));
            var activity = new Activity
            {
                ProcessActivityExecutor = OnExitActivity
            };
            InitialState.Activities ??= new List<Activity>();
            InitialState.Activities.Add(activity);

            return this;
        }

        public ISmartFlowStepBuilder NewStep()
        {
            return SmartFlowBuilder.NewStep();
        }

        public ISmartFlowStepBuilder AssignToUser(Func<string> userId)
        {
            InitialState.OwnerId = userId();

            return this;
        }

        public void Rollback()
        {
            // ignored
        }
    }
}
