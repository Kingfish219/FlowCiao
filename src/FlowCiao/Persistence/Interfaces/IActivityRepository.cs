using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Models.Dto;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IActivityRepository
    {
        Task<List<Activity>> Get(string actorName = default, bool fetchActorContent = false);
        Task<Activity> GetByKey(Guid id = default, string actorName = default);
        Task<Guid> Modify(Activity entity);
        Task<Activity> RegisterActivity(ActivityAssembly activityAssembly);
        Task<IFlowActivity> LoadActivity(string activityFileName);
    }
}
