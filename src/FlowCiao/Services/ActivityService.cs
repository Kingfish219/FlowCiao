using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Utils;

namespace FlowCiao.Services
{
    public class ActivityService : IActivityService
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

        public async Task<FuncResult> RegisterActivity(string actorName, byte[] actorContent)
        {
            if (!actorName.EndsWith(".dll") || actorContent.Length == 0)
            {
                throw new FlowCiaoException("Invalid file");
            }

            var assembly = Assembly.Load(actorContent);
            var extractedActivityTypes = assembly.GetTypes().Where(t => typeof(IFlowActivity).IsAssignableFrom(t)).ToList();
            if (extractedActivityTypes.IsNullOrEmpty())
            {
                throw new FlowCiaoException(
                    "No valid activities found in this file. Please make sure you your activities are accessible and are in the right format");
            }

            foreach (var type in extractedActivityTypes)
            {
                var result = await _activityRepository.RegisterActivity(type.Name, type.FullName, actorContent);
                if (result == default)
                {
                    throw new FlowCiaoPersistencyException("Could not register activities");
                }
            }

            return new FuncResult(true, message: "Activities registered successfully");
        }

        public Task<IFlowActivity> LoadActivity(string activityFileName)
        {
            return _activityRepository.LoadActivity(activityFileName);
        }
    }
}