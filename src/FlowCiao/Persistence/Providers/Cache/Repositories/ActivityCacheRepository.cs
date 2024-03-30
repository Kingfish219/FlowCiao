using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class ActivityCacheRepository : FlowCacheRepository, IActivityRepository
    {
        public ActivityCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<Guid> Modify(Activity entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModfiyActivity(entity);

            return entity.Id;
        }

        public async Task RegisterActivity(ActivityAssembly activityAssembly)
        {
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

            await File.WriteAllBytesAsync(Path.Join(storagePath, activityAssembly.FileName), activityAssembly.FileContent);
        }

        public async Task<IProcessActivity> LoadActivity(string activityFileName)
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
            
            var activity = (IProcessActivity)Activator.CreateInstance(activityType);

            await Task.CompletedTask;
            
            return activity;
        }
    }
}
