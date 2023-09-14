using System;
using System.Threading.Tasks;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class TransitionCacheRepository : FlowCacheRepository, ITransitionRepository
    {
        public TransitionCacheRepository(FlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<Guid> Modify(Transition entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyTransition(entity);

            return entity.Id;
        }

        public async Task AssociateActions(Transition entity, ProcessAction action)
        {
            //return Task.Run(() =>
            //{
            //    var toInsert = new
            //    {
            //        TransitionId = entity.Id,
            //        ActionId = action.Id,
            //        action.Priority
            //    };

            //    using var connection = GetDbConnection();
            //    connection.Open();
            //    connection.Execute(ConstantsProvider.Usp_TransitionAction_Modify, toInsert, commandType: CommandType.StoredProcedure);

            //    return entity.Id;
            //});
        }

        public async Task AssociateActivities(Transition entity, Activity activity)
        {

        }
    }
}
