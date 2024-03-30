using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IStateRepository
    {
        Task<Guid> Modify(State entity);
        Task AssociateActivities(State entity, Activity activity);
    }
}
