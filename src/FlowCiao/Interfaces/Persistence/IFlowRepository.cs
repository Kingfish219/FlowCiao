using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces.Persistence
{
    public interface IFlowRepository
    {
        Task<List<Flow>> Get();
        Task<Flow> GetByKey(Guid id = default, string key = default);
        Task<Guid> Modify(Flow entity);
    }
}
