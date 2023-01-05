using SmartFlow.Core.Models;
using System.Threading.Tasks;
using System;

namespace SmartFlow.Core.Repositories
{
    public class ActionRepository
    {
        private readonly string _connectionString;

        public ActionRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
            DapperHelper.EnsureMappings();
        }

        public Task<Guid> Create(ProcessAction entity)
        {
            return Task.FromResult(Guid.Empty);
        }
    }
}
