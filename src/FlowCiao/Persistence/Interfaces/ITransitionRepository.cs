using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface ITransitionRepository
    {
        Task<Guid> Modify(Transition entity);
        Task AssociateTriggers(Transition entity, Trigger trigger);
        Task AssociateActivities(Transition entity, Activity activity);
    }
}
