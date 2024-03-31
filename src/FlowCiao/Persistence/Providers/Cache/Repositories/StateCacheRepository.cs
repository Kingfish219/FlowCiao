using System;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class StateCacheRepository : FlowCiaoCacheRepository, IStateRepository
    {
        public StateCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<State> GetById(Guid id)
        {
            await Task.CompletedTask;
            
            return FlowHub.States.SingleOrDefault(s => s.Id == id);
        }

        public async Task<Guid> Modify(State entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyState(entity);

            return entity.Id;
        }

        public async Task AssociateActivities(State entity, Activity activity)
        {
            //return Task.Run(() =>
            //{
            //    var toInsert = new
            //    {
            //        StateId = entity.Id,
            //        ActivityId = activity.Id
            //    };

            //    using var connection = GetDbConnection();
            //    connection.Open();
            //    connection.Execute(ConstantsProvider.usp_StateActivity_Modify, toInsert, commandType: CommandType.StoredProcedure);

            //    return entity.Id;
            //});
        }
    }
}
