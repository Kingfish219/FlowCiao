using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System.Threading.Tasks;
using System;

namespace SmartFlow.Core.Services
{
    public class ActivityService
    {
        private readonly ActivityRepository _activityRepository;

        public ActivityService(ActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Guid> Modify(IProcessActivity activity)
        {
            return await _activityRepository.Modify(activity);
        }
    }
}
