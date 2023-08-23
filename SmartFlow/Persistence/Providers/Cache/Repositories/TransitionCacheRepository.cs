using System;
using System.Threading.Tasks;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.Providers.Cache.Repositories
{
    public class TransitionCacheRepository : SmartFlowCacheRepository, ITransitionRepository
    {
        public TransitionCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<Guid> Modify(Transition entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await SmartFlowHub.InsertTransition(entity);

            return entity.Id;
        }

        public Task AssociateActions(Transition entity, ProcessAction action)
        {
            throw new NotImplementedException();

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
