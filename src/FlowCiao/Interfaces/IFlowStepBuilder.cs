using System;
using System.Collections.Generic;
using FlowCiao.Models.Builder;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces
{
    public interface IFlowStepBuilder
    {
        internal void AsInitialStep();
        public IFlowStepBuilder For(State state);
        public IFlowStepBuilder Allow(State state, int trigger, Func<bool> condition = null);
        public IFlowStepBuilder OnEntry<TActivity>() where TActivity : IFlowActivity, new();
        public IFlowStepBuilder OnExit<TActivity>() where TActivity : IFlowActivity, new();
        internal IFlowStepBuilder Build(List<State> states, SerializedStep serializedStep);
        internal FlowStep Build();
    }
}
