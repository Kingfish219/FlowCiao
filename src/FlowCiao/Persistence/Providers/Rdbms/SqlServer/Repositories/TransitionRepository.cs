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
    internal sealed class TransitionRepository : FlowSqlServerRepository, ITransitionRepository
    {
        public TransitionRepository(FlowCiaoDbContext dbContext) : base(dbContext) { }

        public async Task<Transition> GetById(Guid id)
        {
            return await FlowCiaoDbContext.Transitions
                .AsNoTracking()
                .Include(s => s.Activities)
                .Include(s => s.TransitionActivities)
                .Include(s => s.Triggers)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Transition> GetByKey(Guid flowId, Guid fromStateId, Guid toStateId)
        {
            return await FlowCiaoDbContext.Transitions
                .AsNoTracking()
                .Include(s => s.Activities)
                .Include(s => s.TransitionActivities)
                .Include(s => s.Triggers)
                .SingleOrDefaultAsync(a => a.FlowId == flowId && a.FromId == fromStateId && a.ToId == toStateId);
        }

        public async Task<Transition> GetByKey(string flowKey, Guid fromStateId, Guid toStateId)
        {
            return await FlowCiaoDbContext.Transitions
                .AsNoTracking()
                .Include(s => s.Activities)
                .Include(s => s.TransitionActivities)
                .Include(s => s.Triggers)
                .SingleOrDefaultAsync(a => a.Flow.Key == flowKey && a.FromId == fromStateId && a.ToId == toStateId);
        }

        public async Task<Guid> Modify(Transition entity)
        {
            var existed = await GetById(entity.Id) ?? await GetByKey(entity.Flow.Key, entity.From.Id, entity.To.Id);
            if (existed != null)
            {
                if (!existed.TransitionActivities.IsNullOrEmpty())
                {
                    entity.TransitionActivities ??= new List<TransitionActivity>();
                    entity.TransitionActivities.AddRange(existed.TransitionActivities);
                    entity.TransitionActivities = entity.TransitionActivities.DistinctBy(sa => (sa.ActivityId, sa.TransitionId)).ToList();
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
        
        private async Task AssociateActivities(Transition transition)
        {
            if (transition.Activities.IsNullOrEmpty())
            {
                return;
            }

            transition.TransitionActivities ??= new List<TransitionActivity>();
            
            foreach (var association in 
                     from activity in transition.Activities 
                     where !transition.TransitionActivities.Exists(a => a.ActivityId == activity.Id && a.TransitionId == transition.Id) 
                     select new TransitionActivity
                     {
                         TransitionId = transition.Id,
                         ActivityId = activity.Id
                     })
            {
                transition.TransitionActivities.Add(association);
                await CreateNavigationAsync(association);
            }
        }
    }
}
