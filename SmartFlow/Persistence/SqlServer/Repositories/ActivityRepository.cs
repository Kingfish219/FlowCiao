using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Persistence.SqlServer.Repositories
{
    public class ActivityRepository : SmartFlowRepository
    {
        public ActivityRepository(SmartFlowSettings settings) : base(settings)
        {
            
        }

        public Task<Guid> Modify(Activity entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    Executor = entity.ProcessActivityExecutor.ToString()
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_Activity_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }
    }
}
