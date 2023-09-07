using System;
using System.Collections.Generic;
using SmartFlow.Interfaces;
using SmartFlow.Models.Flow;

namespace SmartFlow.Builders
{
    public interface ISmartFlowStepBuilder
    {
        public State InitialState { get; set; }
        internal List<(State, List<ProcessAction>)> AllowedTransitions { get; set; }
        public IProcessActivity OnEntryActivty { get; set; }
        public IProcessActivity OnExitActivity { get; set; }
        public ISmartFlowStepBuilder From(State state);
        public ISmartFlowStepBuilder AllowSelf(List<int> actions);
        public ISmartFlowStepBuilder Allow(State state, List<int> actions);
        public ISmartFlowStepBuilder Allow(State state, int action);
        public ISmartFlowStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new();
        public ISmartFlowStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new();
        public ISmartFlowStepBuilder AssignToUser(Func<string> userId);
    }
}
