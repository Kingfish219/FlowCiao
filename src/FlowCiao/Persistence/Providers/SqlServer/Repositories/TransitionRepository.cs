using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class TransitionRepository : FlowSqlServerRepository, ITransitionRepository
    {
        public TransitionRepository(FlowSettings settings) : base(settings) { }

        public Task<Guid> Modify(Transition entity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    Id = entity.Id == default ? Guid.NewGuid() : entity.Id,
                    ProcessId = entity.FlowId,
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

        public Task AssociateTriggers(Transition entity, Trigger trigger)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    TransitionId = entity.Id,
                    TriggerId = trigger.Id,
                    trigger.Priority
                };

                using var connection = GetDbConnection();
                connection.Open();
                connection.Execute(ConstantsProvider.Usp_TransitionTrigger_Modify, toInsert, commandType: CommandType.StoredProcedure);

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
