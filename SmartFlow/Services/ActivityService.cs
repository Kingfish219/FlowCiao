using System;
using System.Threading.Tasks;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.SqlServer.Repositories;

namespace SmartFlow.Services
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
