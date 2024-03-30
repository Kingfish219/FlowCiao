using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface ITransitionRepository
    {
        Task<Guid> Modify(Transition entity);
        Task AssociateActions(Transition entity, ProcessAction action);
        Task AssociateActivities(Transition entity, Activity activity);
    }
}
