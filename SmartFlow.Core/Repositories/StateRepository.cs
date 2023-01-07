using SmartFlow.Core.Models;
using System.Threading.Tasks;
using System;

namespace SmartFlow.Core.Repositories
{
    public class StateRepository
    {
        private readonly string _connectionString;

        public StateRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        public Task<Guid> Create(State entity)
        {
            return Task.FromResult(Guid.Empty);
        }
    }
}
