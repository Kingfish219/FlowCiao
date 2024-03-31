using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Services
{
    public interface IFlowService
    {
        Task<Guid> Modify(Flow flow);
        Task<List<Flow>> Get();
        Task<Flow> GetByKey(Guid flowId = default, string key = default);
        FlowExecutionStep GenerateFlowStep(Flow flow, State state);
        Task<FlowExecution> Finalize(FlowExecution flowExecution);
    }
}
