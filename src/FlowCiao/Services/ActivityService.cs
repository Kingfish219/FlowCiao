using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Services
{
    public class ActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public Task<List<Activity>> Get(bool fetchActorContent = false)
        {
            return _activityRepository.Get(fetchActorContent);
        }

        public Task<Activity> GetByKey(Guid id = default, string actorName = default)
        {
            return _activityRepository.GetByKey(id, actorName);
        }

        public async Task<Guid> Modify(Activity activity)
        {
            return await _activityRepository.Modify(activity);
        }

        public async Task<Activity> RegisterActivity(string actorName, byte[] actorContent)
        {
            if (!actorName.EndsWith(".dll"))
            {
                throw new FlowCiaoException("Invalid file");
            }

            var result = await _activityRepository.RegisterActivity(actorName, actorContent);
            result.ActorContent = null;

            return result;
        }

        public Task<IFlowActivity> LoadActivity(string activityFileName)
        {
            return _activityRepository.LoadActivity(activityFileName);
        }
    }
}