using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IStateRepository
    {
        Task<State> GetById(Guid id);
        Task<State> GetByKey(int code, string flowKey);
        Task<Guid> Modify(State entity);
    }
}
