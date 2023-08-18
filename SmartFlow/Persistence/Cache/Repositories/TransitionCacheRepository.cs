using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Cache;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.SqlServer.Repositories
{
    public class TransitionCacheRepository : SmartFlowCacheRepository, ITransitionRepository
    {
        public TransitionCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
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

                using var connection = GetDbConnection();
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

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_TransitionAction_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }

        public Task AssociateActivities(Transition entity, Activity activity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    TransitionId = entity.Id,
                    ActivityId = activity.Id
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_TransitionActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }
    }
}
