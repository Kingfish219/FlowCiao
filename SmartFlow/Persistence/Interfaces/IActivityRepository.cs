using SmartFlow.Models.Flow;
using System;
using System.Threading.Tasks;

namespace SmartFlow.Persistence.Interfaces
{
    public interface IActivityRepository
    {
        Task<Guid> Modify(Activity entity);
    }
}
