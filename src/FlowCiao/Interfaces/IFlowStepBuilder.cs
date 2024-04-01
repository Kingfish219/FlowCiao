using System;
using System.Collections.Generic;
using FlowCiao.Models.Builder;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces
{
    public interface IFlowStepBuilder
    {
        public State InitialState { get; set; }
        internal List<Action<Transition>> AllowedBuilders { get; set; }
        protected IFlowActivity OnEntryActivity { get; set; }
        protected IFlowActivity OnExitActivity { get; set; }
        public IFlowStepBuilder For(State state);
        public IFlowStepBuilder Allow(State state, int trigger, Func<bool> condition = null);
        public IFlowStepBuilder OnEntry<TActivity>() where TActivity : IFlowActivity, new();
        public IFlowStepBuilder OnExit<TActivity>() where TActivity : IFlowActivity, new();
        public IFlowStepBuilder Build(List<State> states, JsonStep jsonStep);
        public FlowStep Build();
    }
}
