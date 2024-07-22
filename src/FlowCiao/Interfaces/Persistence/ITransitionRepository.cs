using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Interfaces.Persistence
{
    public interface ITransitionRepository
    {
        Task<Transition> GetById(Guid id);
        Task<Transition> GetByKey(Guid flowId, Guid fromStateId, Guid toStateId);
        Task<Transition> GetByKey(string flowKey, Guid fromStateId, Guid toStateId);
        Task<Guid> Modify(Transition entity);
    }
}
