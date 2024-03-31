using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IFlowRepository
    {
        Task<List<Flow>> Get(Guid flowId = default, string key = default);
        Task<Guid> Modify(Flow entity);
    }
}
