using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    internal sealed class TriggerRepository : FlowSqlServerRepository, ITriggerRepository
    {
        public TriggerRepository(FlowCiaoDbContext dbContext) : base(dbContext) { }

        public async Task<Trigger> GetByKey(int code, Guid transitionId)
        {
            return await FlowCiaoDbContext.Triggers
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Code == code && a.TransitionId == transitionId);
        }

        public async Task<Trigger> GetById(Guid id)
        {
            return await FlowCiaoDbContext.Triggers
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(Trigger entity)
        {
            var existed = await GetByKey(entity.Code, entity.TransitionId);
            if (existed != null)
            {
                await UpdateAsync(entity, existed);
            }
            else
            {
                await CreateAsync(entity);
            }

            return entity.Id;
        }
    }
}
