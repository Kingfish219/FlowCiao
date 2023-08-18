using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Cache;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.SqlServer.Repositories
{
    public class StateCacheRepository : SmartFlowCacheRepository, IStateRepository
    {
        public StateCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public Task<Guid> Modify(State entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.Name
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_State_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }

        public Task AssociateActivities(State entity, Activity activity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    StateId = entity.Id,
                    ActivityId = activity.Id
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.usp_StateActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }
    }
}
