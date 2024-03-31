using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface ITriggerRepository
    {
        Task<Guid> Modify(Trigger entity);
    }
}
