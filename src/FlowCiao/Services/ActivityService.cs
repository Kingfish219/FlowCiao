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

        public Task<List<Activity>> Get()
        {
            return _activityRepository.Get();
        }
        
        public async Task<Guid> Modify(Activity activity)
        {
            return await _activityRepository.Modify(activity);
        }

        public async Task<Activity> RegisterActivity(ActivityAssembly activityAssembly)
        {
            if (!activityAssembly.FileName.EndsWith(".dll"))
            {
                throw new FlowCiaoException("Invalid file");
            }
            
            var result = await _activityRepository.RegisterActivity(activityAssembly);
            result.ActorContent = null;

            return result;
        }

        public Task<IProcessActivity> LoadActivity(string activityFileName)
        {
            return _activityRepository.LoadActivity(activityFileName);
        }
    }
}
