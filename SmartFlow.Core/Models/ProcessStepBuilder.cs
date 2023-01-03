using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessStepBuilder
    {
        public ProcessStepBuilder()
        {
            AllowedTransitions = new List<(State, Action)>();
        }

        public State InitialState { get; set; }
        public List<(State, Action)> AllowedTransitions { get; private set; }
        public System.Action OnEntryAction { get; set; }
        public System.Action OnExitAction { get; set; }

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

        public ProcessStepBuilder OnEntry(System.Action action)
        {
            OnEntryAction = action;

            return this;
        }

        public ProcessStepBuilder OnExit(System.Action action)
        {
            OnExitAction = action;

            return this;
        }

        public Guid Build()
        {
            try
            {
                //var process = _processRepository.Create();

                //foreach (var processStepBuilder in ProcessStepBuilders)
                //{
                //    processStepBuilder.Build();
                //}

                return Guid.NewGuid();
            }
            catch (Exception)
            {
                Rollback();

                throw;
            }
        }

        public void Rollback()
        {

        }
    }
}
