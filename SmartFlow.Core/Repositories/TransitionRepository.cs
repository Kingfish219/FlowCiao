using SmartFlow.Core.Models;
using System.Threading.Tasks;
using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SmartFlow.Core.Db.SqlServer;

namespace SmartFlow.Core.Repositories
{
    public class TransitionRepository
    {
        private readonly string _connectionString;

        public TransitionRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        public Task<Guid> Modify(Transition entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    entity.ProcessId,
                    CurrentStateId = entity.From.Id,
                    NextStateId = entity.To.Id
                };

                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_Transition_Modify, toInsert, commandType: CommandType.StoredProcedure);
                entity.Id = toInsert.Id;

                return entity.Id;
            });
        }

        public Task AssociateActions(Transition entity, ProcessAction action)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    TransitionId = entity.Id,
                    ActionId = action.Id,
                    action.Priority
                };

                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_TransitionAction_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }

        public Task AssociateActivities(Transition entity, Models.Activity activity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    TransitionId = entity.Id,
                    ActivityId = activity.Id
                };

                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_TransitionActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }
    }
}
