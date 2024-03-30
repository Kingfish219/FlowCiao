using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IActionRepository
    {
        Task<Guid> Modify(ProcessAction entity);
    }
}
