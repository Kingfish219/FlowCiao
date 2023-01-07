using SmartFlow.Core.Models;
using System;
using System.Threading.Tasks;

namespace SmartFlow.Core.Repositories
{
    public class ActivityRepository
    {
        private readonly string _connectionString;

        public ActivityRepository(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        public Task<Guid> Create(Activity activity)
        {
            return Task.FromResult(Guid.Empty);
        }
    }
}
