using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Execution;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IFlowExecutionRepository
    {
        Task<List<FlowExecution>> Get(Guid flowId = default);
        Task<FlowExecution> GetById(Guid id);
        Task<Guid> Modify(FlowExecution entity);
    }
}
