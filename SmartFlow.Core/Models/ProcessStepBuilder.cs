using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Models
{
    public class ProcessStepBuilder
    {
        public ProcessStepBuilder(State initialState)
        {
            InitialState = initialState;
            AllowedStatesToTransition = new List<State>();
        }

        public State InitialState { get; set; }
        public List<State> AllowedStatesToTransition { get; set; }

        public ProcessStepBuilder Allow(State state)
        {
            AllowedStatesToTransition.Add(state);

            return this;
        }

        public Guid Build()
        {
            return Guid.NewGuid();
        }
    }
}
