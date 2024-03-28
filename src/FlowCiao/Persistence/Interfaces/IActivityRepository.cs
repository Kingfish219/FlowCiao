using System;
using System.Threading.Tasks;
using FlowCiao.Interfaces;
using FlowCiao.Models.Flow;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IActivityRepository
    {
        Task<Guid> Modify(Activity entity);
        Task RegisterActivity(ActivityAssembly activityAssembly);
        Task<IProcessActivity> LoadActivity(string activityFileName);
    }
}
