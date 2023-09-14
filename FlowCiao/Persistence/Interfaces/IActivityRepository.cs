using System;
using System.Threading.Tasks;
using FlowCiao.Models.Flow;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IActivityRepository
    {
        Task<Guid> Modify(Activity entity);
    }
}
