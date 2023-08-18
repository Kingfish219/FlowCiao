using SmartFlow.Models.Flow;
using System;
using System.Threading.Tasks;

namespace SmartFlow.Persistence.Interfaces
{
    internal interface ITransitionRepository
    {
        Task<Guid> Modify(Transition entity);
        Task AssociateActions(Transition entity, ProcessAction action);
        Task AssociateActivities(Transition entity, Activity activity);
    }
}
