using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using FlowCiao.Exceptions;
using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class ActivityRepository : FlowSqlServerRepository, IActivityRepository
    {
        public ActivityRepository(FlowSettings settings) : base(settings)
        {
        }

        public async Task<List<Activity>> Get(string actorName = default, bool fetchActorContent = false)
        {
            using var connection = GetDbConnection();
            connection.Open();

            var sql = $@"SELECT [Id]
                              ,[Name]
                              ,[ActivityTypeCode]
                              ,[ActorName]
                              {(fetchActorContent ? ",[ActorContent]" : string.Empty)}
                        FROM [FlowCiao].[Activity]
                        WHERE
                            ([ActorName] = @ActorName OR ISNULL(@ActorName, '') = '')";

            var result = (await connection.QueryAsync<Activity>(sql, param: new { ActorName = actorName }))?.ToList();

            return result;
        }

        public async Task<Guid> Modify(Activity entity)
        {
            var toInsert = new
            {
                Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                entity.Name,
                entity.ActivityTypeCode,
                entity.ActorName,
                entity.ActorContent
            };

            using var connection = GetDbConnection();
            
            connection.Open();
            await connection.ExecuteAsync(ConstantsProvider.Usp_Activity_Modify, toInsert,
                commandType: CommandType.StoredProcedure);
            entity.Id = toInsert.Id;

            return entity.Id;
        }

        public async Task<Activity> RegisterActivity(ActivityAssembly activityAssembly)
        {
            var activity = new Activity
            {
                Name = activityAssembly.FileName.Split('.')[^2],
                ActorName = activityAssembly.FileName,
                ActorContent = activityAssembly.FileContent
            };
            activity.Id = await Modify(activity);
            
            return activity;
        }

        public async Task<IFlowActivity> LoadActivity(string activityFileName)
        {
            var existedActivity = (await Get(actorName: activityFileName, true)).SingleOrDefault();
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