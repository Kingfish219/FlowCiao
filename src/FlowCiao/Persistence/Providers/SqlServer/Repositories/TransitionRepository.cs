using System;
using System.Data;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class TransitionRepository : FlowSqlServerRepository, ITransitionRepository
    {
        public TransitionRepository(FlowCiaoDbContext dbContext) : base(dbContext) { }

        public async Task<Transition> GetById(Guid id)
        {
            return await DbContext.Transitions.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(Transition entity)
        {
            var existed = await GetById(entity.Id);
            if (existed != null)
            {
                DbContext.Transitions.Update(entity);
            }
            else
            {
                await DbContext.Transitions.AddAsync(entity);
            }
            
            await DbContext.SaveChangesAsync();

            return entity.Id;
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
                
                // using var connection = GetDbConnection();
                // connection.Open();
                // connection.Execute(ConstantsProvider.Usp_TransitionTrigger_Modify, toInsert, commandType: CommandType.StoredProcedure);

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

                // using var connection = GetDbConnection();
                // connection.Open();
                // connection.Execute(ConstantsProvider.Usp_TransitionActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }
    }
}
