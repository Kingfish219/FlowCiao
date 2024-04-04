using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface ITransitionRepository
    {
        Task<Transition> GetById(Guid id);
        Task<Guid> Modify(Transition entity);
    }
}
