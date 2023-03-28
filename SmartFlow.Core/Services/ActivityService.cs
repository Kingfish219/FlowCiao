using SmartFlow.Core.Models;
using System.Threading.Tasks;
using System;
using SmartFlow.Core.Persistence.SqlServer.Repositories;

namespace SmartFlow.Core.Services
{
    public class ActivityService
    {
        private readonly ActivityRepository _activityRepository;

        public ActivityService(ActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Guid> Modify(Activity activity)
        {
            return await _activityRepository.Modify(activity);
        }
    }
}
