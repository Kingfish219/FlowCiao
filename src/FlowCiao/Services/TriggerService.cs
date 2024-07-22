using System;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Models.Core;

namespace FlowCiao.Services
{
    public class TriggerService
    {
        private readonly ITriggerRepository _triggerRepository;

        public TriggerService(ITriggerRepository triggerRepository)
        {
            _triggerRepository = triggerRepository;
        }

        public async Task<Guid> Modify(Trigger trigger)
        {
            return await _triggerRepository.Modify(trigger);
        }
    }
}
