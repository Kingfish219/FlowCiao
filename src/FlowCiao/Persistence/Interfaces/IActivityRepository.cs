using System;
using System.Threading.Tasks;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IActivityRepository
    {
        Task<Guid> Modify(Activity entity);
        Task RegisterActivity(ActivityAssembly activityAssembly);
        Task<IProcessActivity> LoadActivity(string activityFileName);
    }
}
