using System;
using System.Collections.Generic;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Builders
{
    public class StateMachineStepBuilder : IStateMachineStepBuilder
    {
        public IStateMachineBuilder ProcessBuilder { get; set; }

        public StateMachineStepBuilder(IStateMachineBuilder processBuilder)
        {
            ProcessBuilder = processBuilder;
            AllowedTransitions = new List<(State, List<ProcessAction>)>();
        }

        public State InitialState { get; set; }
        public List<(State, List<ProcessAction>)> AllowedTransitions { get; set; }
        public IProcessActivity OnEntryActivty { get; set; }
        public IProcessActivity OnExitActivity { get; set; }

        public StateMachineStepBuilder From(State state)
        {
            InitialState = state;

            return this;
        }

        public StateMachineStepBuilder Allow(State state, List<ProcessAction> actions)
        {
            AllowedTransitions.Add((state, actions));

            return this;
        }

        public StateMachineStepBuilder Allow(State state, ProcessAction action)
        {
            var actions = new List<ProcessAction>
            {
                action
            };

            AllowedTransitions.Add((state, actions));

            return this;
        }

        public StateMachineStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new()
        {
            OnEntryActivty = (Activity)Activator.CreateInstance(typeof(Activity));

            return this;
        }

        public StateMachineStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new()
        {
            OnExitActivity = (Activity)Activator.CreateInstance(typeof(Activity));

            return this;
        }

        public IStateMachineStepBuilder NewStep()
        {
            return ProcessBuilder.NewStep();
        }

        public void Rollback()
        {

        }
    }
}
