using System;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Builder;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces.Builder
{
    public interface IFlowStepBuilder
    {
        internal void AsInitialStep();
        public IFlowStepBuilder For(State state);
        public IFlowStepBuilder Allow(State state, int trigger, Func<bool> condition = null);
        public IFlowStepBuilder Allow(State state, Trigger trigger, Func<bool> condition = null);
        public IFlowStepBuilder OnEntry<TActivity>() where TActivity : IFlowActivity, new();
        public IFlowStepBuilder OnExit<TActivity>() where TActivity : IFlowActivity, new();
        internal FlowStep Build(Guid flowId);
        internal Task<FuncResult> Persist(Flow flow);
    }
}
