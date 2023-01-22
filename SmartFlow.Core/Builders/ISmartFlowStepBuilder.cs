using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public interface ISmartFlowStepBuilder
    {
        public State InitialState { get; set; }
        public List<(State, List<ProcessAction>)> AllowedTransitions { get; set; }
        public IProcessActivity OnEntryActivty { get; set; }
        public IProcessActivity OnExitActivity { get; set; }
        public ISmartFlowStepBuilder NewStep();
        public ISmartFlowStepBuilder From(State state);
        public ISmartFlowStepBuilder AllowSelf(List<ProcessAction> actions);
        public ISmartFlowStepBuilder Allow(State state, List<ProcessAction> actions);
        public ISmartFlowStepBuilder Allow(State state, ProcessAction action);
        public ISmartFlowStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new();
        public ISmartFlowStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new();
        public ISmartFlowStepBuilder AssignToUser(Func<string> userId);
    }
}
