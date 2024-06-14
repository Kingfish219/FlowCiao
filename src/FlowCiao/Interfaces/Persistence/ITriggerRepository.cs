using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces.Persistence
{
    public interface ITriggerRepository
    {
        Task<Trigger> GetByKey(int code, Guid transitionId);
        Task<Trigger> GetById(Guid id);
        Task<Guid> Modify(Trigger entity);
    }
}
