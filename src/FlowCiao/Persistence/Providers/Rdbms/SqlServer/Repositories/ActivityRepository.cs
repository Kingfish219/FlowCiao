using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    public class ActivityRepository : FlowSqlServerRepository, IActivityRepository
    {
        public ActivityRepository(FlowCiaoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Activity>> Get(string actorName = default, bool fetchActorContent = false)
        {
            if (fetchActorContent)
            {
                return await DbContext.Activities
                    .Where(a =>
                        string.IsNullOrWhiteSpace(actorName) || actorName.Equals(a.ActorName))
                    .ToListAsync();
            }
            
            return await DbContext.Activities
                .Where(a =>
                    string.IsNullOrWhiteSpace(actorName) || actorName.Equals(a.ActorName))
                .Select(activity => new Activity
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    ActorName = activity.ActorName,
                    ActivityType = activity.ActivityType
                })
                .ToListAsync();
        }
        
        public async Task<Activity> GetByKey(Guid id = default, string actorName = default)
        {
            return await DbContext.Activities.SingleOrDefaultAsync(a =>
                (a.Id == default || a.Id == id) &&
                (string.IsNullOrWhiteSpace(actorName) || a.ActorName.Equals(actorName, StringComparison.InvariantCultureIgnoreCase)));
        }

        public async Task<Guid> Modify(Activity entity)
        {
            var existed = await GetByKey(entity.Id);
            if (existed != null)
            {
                DbContext.Activities.Update(entity);
            }
            else
            {
                await DbContext.Activities.AddAsync(entity);
            }
            
            await DbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<Activity> RegisterActivity(string actorName, byte[] actorContent)
        {
            var activity = new Activity
            {
                Name = actorName.Split('.')[^2],
                ActorName = actorName,
                ActorContent = actorContent
            };
            activity.Id = await Modify(activity);
            
            return activity;
        }

        public async Task<IFlowActivity> LoadActivity(string activityFileName)
        {
            var existedActivity = await GetByKey(actorName: activityFileName);
            if (existedActivity is null)
            {
                throw new FlowCiaoPersistencyException($"Could not find assembly with name: {activityFileName}");
            }

            var assembly = Assembly.Load(existedActivity.ActorContent);
            var activityType = assembly.GetType(activityFileName);
            if (activityType is null)
            {
                throw new FlowCiaoException(
                    $"Could not load assembly with name: {activityFileName}. Content may be corrupted");
            }

            var activity = (IFlowActivity)Activator.CreateInstance(activityType);

            return activity;
        }
    }
}