
using SmartFlow.Core.Models;
using System.Threading.Tasks;
using System;

namespace SmartFlow.Core.Repositories
{
    public class TransitionRepository
    {
        private readonly string _connectionString;

        public TransitionRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
            DapperHelper.EnsureMappings();
        }

        public Task<Guid> Create(Transition entity)
        {
            return Task.FromResult(Guid.Empty);
        }
    }
}
