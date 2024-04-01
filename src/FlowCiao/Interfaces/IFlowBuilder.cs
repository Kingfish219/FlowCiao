using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces
{
    public interface IFlowBuilder
    {
        List<IFlowStepBuilder> StepBuilders { get; set; }
        IFlowStepBuilder InitialStepBuilder { get; set; }
        IFlowBuilder Initial(Action<IFlowStepBuilder> action);
        IFlowBuilder NewStep(Action<IFlowStepBuilder> action);
        Flow Build<T>(Action<IFlowBuilder> action) where T : IFlowPlanner, new();
        Flow Build<T>() where T : IFlowPlanner, new();
        Task<Flow> BuildFromJsonAsync(string json);
    }
}
