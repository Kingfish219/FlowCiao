using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.SqlServer.Repositories
{
    public class ActionRepository : SmartFlowSqlServerRepository, IActionRepository
    {
        public ActionRepository(SmartFlowSettings settings) : base(settings) { }

        public Task<Guid> Modify(ProcessAction entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.Name,
                    entity.ActionTypeCode,
                    entity.ProcessId
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_Action_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }
    }
}
