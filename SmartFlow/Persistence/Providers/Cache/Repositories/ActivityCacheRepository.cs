using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.Providers.Cache.Repositories
{
    public class ActivityCacheRepository : SmartFlowCacheRepository, IActivityRepository
    {
        public ActivityCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public Task<Guid> Modify(Activity entity)
        {
            throw new NotImplementedException();

            //return Task.Run(() =>
            //{
            //    var toInsert = new
            //    {
            //        Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
            //        Executor = entity.ProcessActivityExecutor.ToString()
            //    };

            //    using var connection = GetDbConnection();
            //    connection.Open();
            //    connection.Execute(ConstantsProvider.Usp_Activity_Modify, toInsert, commandType: CommandType.StoredProcedure);
            //    entity.Id = toInsert.Id;

            //    return entity.Id;
            //});
        }
    }
}
