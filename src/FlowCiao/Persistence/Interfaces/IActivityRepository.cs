using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IActivityRepository
    {
        Task<List<Activity>> Get(string actorName = default, bool fetchActorContent = false);
        Task<Guid> Modify(Activity entity);
        Task<Activity> RegisterActivity(ActivityAssembly activityAssembly);
        Task<IProcessActivity> LoadActivity(string activityFileName);
    }
}
