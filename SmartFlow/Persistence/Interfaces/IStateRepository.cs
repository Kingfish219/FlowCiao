using SmartFlow.Models.Flow;
using System;
using System.Threading.Tasks;

namespace SmartFlow.Persistence.Interfaces
{
    public interface IStateRepository
    {
        Task<Guid> Modify(State entity);
        Task AssociateActivities(State entity, Activity activity);
    }
}
