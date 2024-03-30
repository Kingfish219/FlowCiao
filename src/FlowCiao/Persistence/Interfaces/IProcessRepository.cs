using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IProcessRepository
    {
        Task<List<Process>> Get(Guid processId = default, string key = default);
        Task<Guid> Modify(Process entity);
    }
}
