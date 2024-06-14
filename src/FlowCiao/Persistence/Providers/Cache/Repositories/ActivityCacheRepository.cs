using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Models;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal sealed class ActivityCacheRepository : FlowCiaoCacheRepository, IActivityRepository
    {
        public ActivityCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<List<Activity>> Get(bool fetchActorContent = false)
        {
            await Task.CompletedTask;
            
            return FlowHub.Activities;
        }

        public async Task<Activity> GetByKey(Guid id = default, string actorName = default)
        {
            await Task.CompletedTask;

            return FlowHub.Activities.SingleOrDefault(a =>
                (a.Id == default || a.Id == id) &&
                (string.IsNullOrWhiteSpace(actorName) || a.ActorName.Equals(actorName, StringComparison.InvariantCultureIgnoreCase)));
        }

        public async Task<Guid> Modify(Activity entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyActivity(entity);

            return entity.Id;
        }

        public async Task<FuncResult<Guid>> RegisterActivity(string name, string actorName, byte[] actorContent)
        {
            var activity = new Activity
            {
                Name = name,
                ActorName = actorName,
                ActorContent = actorContent
            };
            activity.Id = await Modify(activity);
            
            var storagePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"FlowCiao\Assembly");
            if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(storagePath)))
            {
                throw new FlowCiaoPersistencyException("Could not get ProgramsData path in order to save the file");
            }

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            await File.WriteAllBytesAsync(Path.Join(storagePath, actorName), actorContent);

            return new FuncResult<Guid>(true, data: activity.Id);
        }

        public async Task<IFlowActivity> LoadActivity(string activityFileName)
        {
            var storagePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"FlowCiao\Assembly");
            if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(storagePath)))
            {
                throw new DirectoryNotFoundException("Could not get ProgramsData path in order to save the file");
            }

            Type activityType = null;
            foreach (var file in Directory.GetFiles(storagePath))
            {
                var assembly = Assembly.LoadFrom(file);
                activityType = assembly.GetType(activityFileName);
                if (activityType is not null)
                {
                    break;
                }
            }

            if (activityType is null)
            {
                throw new FileNotFoundException($"Could not find assembly with name: {activityFileName}");
            }
            
            var activity = (IFlowActivity)Activator.CreateInstance(activityType);

            await Task.CompletedTask;
            
            return activity;
        }
    }
}
