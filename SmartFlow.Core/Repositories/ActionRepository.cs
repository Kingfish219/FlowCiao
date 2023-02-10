using SmartFlow.Core.Models;
using System.Threading.Tasks;
using System;
using SmartFlow.Core.Db.SqlServer;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace SmartFlow.Core.Repositories
{
    public class ActionRepository
    {
        private readonly string _connectionString;

        public ActionRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

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

                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_Action_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }
    }
}
