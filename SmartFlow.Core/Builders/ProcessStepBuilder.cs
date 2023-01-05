using System;
using System.Collections.Generic;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Builders
{
    public class ProcessStepBuilder
    {
        public ProcessBuilder ProcessBuilder { get; set; }

        public ProcessStepBuilder(ProcessBuilder processBuilder)
        {
            ProcessBuilder = processBuilder;
            AllowedTransitions = new List<(State, List<ProcessAction>)>();
        }

        public State InitialState { get; set; }
        public List<(State, List<ProcessAction>)> AllowedTransitions { get; private set; }
        public IProcessActivity OnEntryActivty { get; set; }
        public IProcessActivity OnExitActivity { get; set; }

        public ProcessStepBuilder From(State state)
        {
            InitialState = state;

            return this;
        }

        public ProcessStepBuilder Allow(State state, List<ProcessAction> actions)
        {
            AllowedTransitions.Add((state, actions));

            return this;
        }

        public ProcessStepBuilder Allow(State state, ProcessAction action)
        {
            var actions = new List<ProcessAction>
            {
                action
            };

            AllowedTransitions.Add((state, actions));

            return this;
        }

        public ProcessStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new()
        {
            OnEntryActivty = (Activity)Activator.CreateInstance(typeof(Activity));

            return this;
        }

        public ProcessStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new()
        {
            OnExitActivity = (Activity)Activator.CreateInstance(typeof(Activity));

            return this;
        }

        public Process Build()
        {
            try
            {
                return ProcessBuilder.Build();
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public ProcessStepBuilder NewStep()
        {
            return ProcessBuilder.NewStep();
        }

        public void Rollback()
        {

        }
    }
}
