using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Interfaces;
using FlowCiao.Models.Builder.Json;
using FlowCiao.Models.Core;

namespace FlowCiao.Builders
{
    public interface IFlowBuilder
    {
        List<IFlowStepBuilder> StepBuilders { get; set; }
        IFlowStepBuilder InitialStepBuilder { get; set; }
        IFlowBuilder Initial(Action<IFlowStepBuilder> action);
        IFlowBuilder NewStep(Action<IFlowStepBuilder> action);
        Process Build<T>(Action<IFlowBuilder> action) where T : IFlow, new();
        Process Build<T>() where T : IFlow, new();
        Task<Process> Build(JsonFlow jsonFlow);
    }
}
