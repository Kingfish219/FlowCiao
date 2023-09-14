using System;
using System.Threading.Tasks;
using FlowCiao.Models.Flow;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IActionRepository
    {
        Task<Guid> Modify(ProcessAction entity);
    }
}
