using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces
{
    public interface IFlowBuilder
    {
        IFlowBuilder Initial(Action<IFlowStepBuilder> action);
        IFlowBuilder NewStep(Action<IFlowStepBuilder> action);
        Flow Build(string flowKey, Action<IFlowBuilder> build);
        Task<Flow> BuildAsync(string flowKey, Action<IFlowBuilder> build);
        Flow Build<T>() where T : IFlowPlanner, new();
        Task<Flow> BuildAsync<T>() where T : IFlowPlanner, new();
        Flow Build(IFlowPlanner flowPlanner);
        Task<Flow> BuildAsync(IFlowPlanner flowPlanner);
    }
}
