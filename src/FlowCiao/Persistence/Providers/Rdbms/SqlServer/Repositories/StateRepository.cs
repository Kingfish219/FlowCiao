using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Utils;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    public class StateRepository : FlowSqlServerRepository, IStateRepository
    {
        public StateRepository(FlowCiaoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<State> GetById(Guid id)
        {
            return await FlowCiaoDbContext.States
                .AsNoTracking()
                .Include(s => s.Activities)
                .Include(s => s.StateActivities)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<State> GetByKey(int code, string flowKey)
        {
            return await FlowCiaoDbContext.States
                .AsNoTracking()
                .Include(s => s.Activities)
                .Include(s => s.StateActivities)
                .SingleOrDefaultAsync(state => state.Code == code && state.Flow.Key == flowKey);
        }

        public async Task<Guid> Modify(State entity)
        {
            var existed = await GetById(entity.Id) ?? await GetByKey(entity.Code, entity.Flow.Key);
            if (existed != null)
            {
                if (!existed.StateActivities.IsNullOrEmpty())
                {
                    entity.StateActivities ??= new List<StateActivity>();
                    entity.StateActivities.AddRange(existed.StateActivities);
                    entity.StateActivities = entity.StateActivities.DistinctBy(sa => (sa.ActivityId, sa.StateId)).ToList();
                }
                
                await UpdateAsync(entity, existed);
            }
            else
            {
                await CreateAsync(entity);
            }

            await AssociateActivities(entity);

            return entity.Id;
        }

        private async Task AssociateActivities(State state)
        {
            if (state.Activities.IsNullOrEmpty())
            {
                return;
            }

            state.StateActivities ??= new List<StateActivity>();
            
            foreach (var association in 
                     from activity in state.Activities 
                     where !state.StateActivities.Exists(a => a.ActivityId == activity.Id && a.StateId == state.Id) 
                     select new StateActivity
                     {
                         StateId = state.Id,
                         ActivityId = activity.Id
                     })
            {
                state.StateActivities.Add(association);
                await CreateNavigationAsync(association);
            }
        }
    }
}