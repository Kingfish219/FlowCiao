using SmartFlow.Core.Models;
using System.Threading.Tasks;
using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SmartFlow.Core.Repositories
{
    public class StateRepository
    {
        private readonly string _connectionString;

        public StateRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        public Task<Guid> Create(State entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.Name
                };

                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                connection.Execute("[SmartFlow].[usp_State_Insert]", toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }
    }
}
