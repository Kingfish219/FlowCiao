using System;
using System.Threading.Tasks;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.Providers.Cache.Repositories
{
    public class StateCacheRepository : SmartFlowCacheRepository, IStateRepository
    {
        public StateCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<Guid> Modify(State entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await SmartFlowHub.ModifyState(entity);

            return entity.Id;
        }

        public async Task AssociateActivities(State entity, Activity activity)
        {
            //return Task.Run(() =>
            //{
            //    var toInsert = new
            //    {
            //        StateId = entity.Id,
            //        ActivityId = activity.Id
            //    };

            //    using var connection = GetDbConnection();
            //    connection.Open();
            //    connection.Execute(ConstantsProvider.usp_StateActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

            //    return entity.Id;
            //});
        }
    }
}
