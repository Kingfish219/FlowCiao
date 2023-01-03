using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessStepBuilder
    {
        public WorkflowBuilder WorkflowBuilder { get; set; }

        public ProcessStepBuilder(WorkflowBuilder workflowBuilder)
        {
            WorkflowBuilder = workflowBuilder;
            AllowedTransitions = new List<(State, Action)>();
        }

        public State InitialState { get; set; }
        public List<(State, Action)> AllowedTransitions { get; private set; }
        public IProcessActivity OnEntryFunc { get; set; }
        public IProcessActivity OnExitFunc { get; set; }

        public ProcessStepBuilder From(State state)
        {
            InitialState = state;

            return this;
        }

        public ProcessStepBuilder Allow(State state, Action action)
        {
            AllowedTransitions.Add((state, action));

            return this;
        }

        public ProcessStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new()
        {
            OnEntryFunc = (Activity)Activator.CreateInstance(typeof(Activity));

            return this;
        }

        public ProcessStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new()
        {
            OnExitFunc = (Activity)Activator.CreateInstance(typeof(Activity));

            return this;
        }

        public Process Build()
        {
            try
            {
                return WorkflowBuilder.Build();
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public ProcessStepBuilder NewStep()
        {
            return WorkflowBuilder.NewStep();
        }

        public void Rollback()
        {

        }
    }
}
