using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class StateRepository : FlowSqlServerRepository, IStateRepository
    {
        public StateRepository(FlowCiaoDbContext dbContext) : base(dbContext) { }
        
        public async Task<State> GetById(Guid id)
        {
            return await DbContext.States.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(State entity)
        {
            var existed = await GetById(entity.Id);
            if (existed != null)
            {
                DbContext.States.Update(entity);
            }
            else
            {
                await DbContext.States.AddAsync(entity);
            }
            
            await DbContext.SaveChangesAsync();

            return entity.Id;
        }

        public Task AssociateActivities(State entity, Activity activity)
        {
            return Task.Run(() =>
            {
                var toInsert = new
                {
                    StateId = entity.Id,
                    ActivityId = activity.Id
                };

                // using var connection = GetDbConnection();
                // connection.Open();
                // connection.Execute(ConstantsProvider.usp_StateActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

                return entity.Id;
            });
        }
    }
}
