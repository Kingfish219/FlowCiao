using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    public class TransitionRepository : FlowSqlServerRepository, ITransitionRepository
    {
        public TransitionRepository(FlowCiaoDbContext dbContext) : base(dbContext) { }

        public async Task<Transition> GetById(Guid id)
        {
            return await FlowCiaoDbContext.Transitions
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(Transition entity)
        {
            var existed = await GetById(entity.Id);
            FlowCiaoDbContext.Entry(entity).State = EntityState.Unchanged;
            if (existed != null)
            {
                FlowCiaoDbContext.Transitions.Update(entity);
            }
            else
            {
                await FlowCiaoDbContext.Transitions.AddAsync(entity);
            }
            
            await FlowCiaoDbContext.SaveChangesAsync();

            return entity.Id;
        }
    }
}
