using System;
using System.Collections.Generic;
using FlowCiao.Interfaces;
using FlowCiao.Models.Flow;

namespace FlowCiao.Builders
{
    public interface IFlowStepBuilder
    {
        public State InitialState { get; set; }
        internal List<Action<Transition>> AllowedTransitionsBuilders { get; set; }
        public IProcessActivity OnEntryActivty { get; set; }
        public IProcessActivity OnExitActivity { get; set; }
        public IFlowStepBuilder From(State state);
        public IFlowStepBuilder AllowSelf(List<int> actions);
        public IFlowStepBuilder Allow(State state, List<int> actions);
        public IFlowStepBuilder Allow(State state, int action, Func<bool> condition = null);
        public IFlowStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new();
        public IFlowStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new();
        public IFlowStepBuilder AssignToUser(Func<string> userId);
    }
}
