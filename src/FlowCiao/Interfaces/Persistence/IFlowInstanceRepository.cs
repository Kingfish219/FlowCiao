using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Execution;

namespace FlowCiao.Interfaces.Persistence
{
    public interface IFlowInstanceRepository
    {
        Task<List<FlowInstance>> Get(Guid flowId = default);
        Task<FlowInstance> GetById(Guid id);
        Task<Guid> Modify(FlowInstance entity);
        Task<Guid> Update(FlowInstance entity);
    }
}
