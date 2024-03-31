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
        Task<Activity> GetByKey(Guid id = default, string actorName = default);
        Task<Guid> Modify(Activity entity);
        Task<Activity> RegisterActivity(string actorName, byte[] actorContent);
        Task<IFlowActivity> LoadActivity(string activityFileName);
    }
}
