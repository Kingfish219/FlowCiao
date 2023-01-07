using SmartFlow.Core.Models;
using System.Collections.Generic;

namespace SmartFlow.Core.Builders
{
    public interface IStateMachineStepBuilder
    {
        internal State InitialState { get; set; }
        internal List<(State, List<ProcessAction>)> AllowedTransitions { get; set; }
        internal IProcessActivity OnEntryActivty { get; set; }
        internal IProcessActivity OnExitActivity { get; set; }
        public IStateMachineStepBuilder NewStep();
        public IStateMachineStepBuilder From(State state);
        public IStateMachineStepBuilder AllowSelf(List<ProcessAction> actions);
        public IStateMachineStepBuilder Allow(State state, List<ProcessAction> actions);
        public IStateMachineStepBuilder Allow(State state, ProcessAction action);
        public IStateMachineStepBuilder OnEntry<Activity>() where Activity : IProcessActivity, new();
        public IStateMachineStepBuilder OnExit<Activity>() where Activity : IProcessActivity, new();
    }
}
